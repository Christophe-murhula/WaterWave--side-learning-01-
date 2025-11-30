using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMissileLunch : MonoBehaviour
{
    [SerializeField] GameObject Missile;
    [SerializeField] Transform CanonTransform;
    [SerializeField][Range(0f, float.PositiveInfinity)] float pushbackOnLaunch;
    Rigidbody2D myBody;

    Vector3 DEFAULT_CANON_LOCAL_POS;
    const float missileSpeed = 10f;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        DEFAULT_CANON_LOCAL_POS = CanonTransform.localPosition;
    }

    void FixedUpdate()
    {
        RepositionCanon();
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

        // set missile position and rotation to the canon and body values
        missile.transform.position = CanonTransform.position;
        missileBody.rotation = myBody.rotation - 90f;

        // missile velocity
        missileBody.linearVelocityY = transform.right.y * missileSpeed;
        missileBody.linearVelocityX = transform.right.x * missileSpeed;

        // slight push back on the player
        myBody.AddForce(-pushbackOnLaunch * transform.right, ForceMode2D.Impulse);

        // slight more push on canon
        var xOffset = 0.3f;
        CanonTransform.localPosition = new Vector3(DEFAULT_CANON_LOCAL_POS.x - xOffset, DEFAULT_CANON_LOCAL_POS.y, DEFAULT_CANON_LOCAL_POS.z);
    }

    void RepositionCanon()
    {
        if (CanonTransform.localPosition.x != DEFAULT_CANON_LOCAL_POS.x)
        {
            var lerpX = Mathf.Lerp(CanonTransform.localPosition.x, DEFAULT_CANON_LOCAL_POS.x, 0.1f);

            CanonTransform.localPosition = new Vector3(lerpX, DEFAULT_CANON_LOCAL_POS.y, DEFAULT_CANON_LOCAL_POS.z);
        }
    }
}
