using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] EdgeCollider2D waterCollider;
    [SerializeField] float linearSpeed;

    Rigidbody2D myBody;
    Vector2 linearSpeedMultiplier = new(0f, 0f);
    float currentAngle, targetAngle;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        currentAngle = targetAngle = myBody.rotation;
    }

    void FixedUpdate()
    {
        // Lerp angle between target and current angle
        if (currentAngle != targetAngle)
        {
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, .2f);
            myBody.SetRotation(currentAngle);
        }

        // Lerp velocity to simulate inertia
        myBody.linearVelocityX = Mathf.Lerp(myBody.linearVelocityX, linearSpeed * linearSpeedMultiplier.x, .07f);
        myBody.linearVelocityY = Mathf.Lerp(myBody.linearVelocityY, linearSpeed * linearSpeedMultiplier.y, .07f);

        // add gravity if we are out of water, reset it otherwise
        if (IsOutofWater())
        {
            myBody.gravityScale = 1.5f;
            myBody.linearVelocity = Vector2.zero;
        }
        else
        {
            if (myBody.gravityScale != 0f)
            {
                myBody.gravityScale = 0f;
            }
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

    bool IsOutofWater()
    {
        var waterTopCoordinate = waterCollider.bounds.max.y;

        return waterTopCoordinate < transform.position.y;
    }
}
