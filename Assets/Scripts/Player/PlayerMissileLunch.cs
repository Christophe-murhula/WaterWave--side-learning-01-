using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMissileLunch : MonoBehaviour
{
    [SerializeField] GameObject Missile;
    [SerializeField] Transform CanonTransform;
    Rigidbody2D myBody;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LaunchMissile();
        }
    }

    void LaunchMissile()
    {
        print(transform.right);
        GameObject missile = Instantiate(Missile);
        missile.transform.position = CanonTransform.position;
        Rigidbody2D missileBody = missile.GetComponent<Rigidbody2D>();
        missileBody.rotation = myBody.rotation - 90f;

        var missileSpeed = 10f;
        if (myBody.linearVelocity.magnitude == 0f)
        {
            // launch upward
            missileBody.linearVelocityY = missileSpeed;
        }
        else
        {
            missileBody.linearVelocityY = transform.right.y * missileSpeed;
            missileBody.linearVelocityX = transform.right.x * missileSpeed;
        }
    }
}
