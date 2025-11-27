using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMissileLunch : MonoBehaviour
{
    [SerializeField] GameObject Missile;
    [SerializeField] Transform CanonTransform;
    Rigidbody2D myBody;

    float missileSpeed = 10f;

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
        GameObject missile = Instantiate(Missile);
        Rigidbody2D missileBody = missile.GetComponent<Rigidbody2D>();

        missile.transform.position = CanonTransform.position;
        missileBody.rotation = myBody.rotation - 90f;

        missileBody.linearVelocityY = transform.right.y * missileSpeed;
        missileBody.linearVelocityX = transform.right.x * missileSpeed;
    }
}
