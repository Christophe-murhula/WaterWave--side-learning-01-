using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] EdgeCollider2D waterCollider;
    [SerializeField] float linearSpeed;

    Rigidbody2D myBody;
    CapsuleCollider2D myBodyCollider;
    CircleCollider2D myRadiusTriggerer;
    Vector2 linearSpeedMultiplier = new(0f, 0f);
    float currentAngle, targetAngle;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        currentAngle = targetAngle = myBody.rotation;

        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myRadiusTriggerer = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        // Lerp angle between target and current angle
        if (currentAngle != targetAngle)
        {
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, .07f);
            myBody.SetRotation(currentAngle);
        }
        // Lerp velocity to simulate inertia
        myBody.linearVelocityX = Mathf.Lerp(myBody.linearVelocityX, linearSpeed * linearSpeedMultiplier.x, .07f);
        myBody.linearVelocityY = Mathf.Lerp(myBody.linearVelocityY, linearSpeed * linearSpeedMultiplier.y, .07f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        bool isWater = collision.gameObject.layer == LayerMask.NameToLayer("Water");
        if (isWater && myBody.bodyType != RigidbodyType2D.Dynamic)
        {
            myBody.bodyType = RigidbodyType2D.Dynamic;
        }

        // Generate wave
        bool isSpring = collision.gameObject.layer == LayerMask.NameToLayer("Springs");
        if (isSpring)
        {
            Rigidbody2D springBody = collision.gameObject.GetComponent<Rigidbody2D>();
            springBody.linearVelocityY = 10f;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        bool isWater = collision.gameObject.layer == LayerMask.NameToLayer("Water");
        if (isWater && myBody.bodyType != RigidbodyType2D.Dynamic)
        {
            myBody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        bool isWater = collision.gameObject.layer == LayerMask.NameToLayer("Water");
        if (isWater && myBody.bodyType != RigidbodyType2D.Kinematic)
        {
            myBody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        linearSpeedMultiplier = context.ReadValue<Vector2>().normalized;

        // Calculate angle according to input
        if (linearSpeedMultiplier.magnitude == 0)
        {
            // Default facing up
            targetAngle = Mathf.Atan2(1f, 0f);
        }
        else
        {
            targetAngle = Mathf.Atan2(linearSpeedMultiplier.y, linearSpeedMultiplier.x);
        }
        targetAngle *= Mathf.Rad2Deg;
    }
}
