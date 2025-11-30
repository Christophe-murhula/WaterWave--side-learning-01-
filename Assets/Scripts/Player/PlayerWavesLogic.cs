using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWavesLogic : MonoBehaviour
{
    ParticleSystem myParticles;
    Rigidbody2D myBody;

    void Start()
    {
        myParticles = GetComponent<ParticleSystem>();
        myBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Create 360 degrees destruction waves
    /// </summary>
    /// <param name="context">Input Action callback context</param>
    public void OnWave360(InputAction.CallbackContext context)
    {
        // create wave when action performed (held enough time) AND not moving
        if (context.phase == InputActionPhase.Performed && myBody.linearVelocity.magnitude <= .5f)
        {
            myParticles.Play();
        }
    }
}
