using UnityEngine;

public class MissileDestruction : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticleSystem;

    ParticleSystem myParticles;
    Rigidbody2D myBody;

    const float LIFETIME = 7f; // destruction after this number of seconds
    bool isExplosionGameobjectActive = false;
    float watercontact_countdown = 0f;

    void Start()
    {
        myParticles = GetComponent<ParticleSystem>();
        myBody = GetComponent<Rigidbody2D>();

        // set myParticles trigger to water shape
        Component watershapeComponent = FindFirstObjectByType<SpringsController>();
        myParticles.trigger.AddCollider(watershapeComponent);

        // set destruction eventually at lifetime
        Destroy(gameObject, LIFETIME);
    }

    void Update()
    {
        var deltatime = Time.deltaTime;

        if (watercontact_countdown > 0f)
        {
            watercontact_countdown -= deltatime;
        }
        else
        {
            if (isExplosionGameobjectActive)
            {
                KillAfterCountdown();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        bool isWater = collision.gameObject.layer == LayerMask.NameToLayer("Water");

        if (isWater && !isExplosionGameobjectActive)
        {
            //activate the explosion game object
            explosionParticleSystem.gameObject.SetActive(true);
            isExplosionGameobjectActive = true;

            // set this particle system shape arc to 360
            var particlesShape = myParticles.shape;
            particlesShape.arc = 360f;

            // set alpha to 0
            var transparentColor = new Color(0f, 0f, 0f, 0f);
            GetComponent<SpriteRenderer>().color = transparentColor;

            // set velocity to zero
            myBody.linearVelocity = Vector2.zero;

            // start countdown of 3s
            watercontact_countdown = 2f;
        }
    }

    void KillAfterCountdown()
    {
        if (myParticles.isPlaying)
        {
            myParticles.Stop();
        }
        if (myParticles.particleCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
