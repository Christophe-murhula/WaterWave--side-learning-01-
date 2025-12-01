using UnityEngine;

public class MissileDestruction : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticleSystem;
    [SerializeField] ParticleSystem explosionOnIcebergParticleSystem;
    ParticleSystem currentExplosion;

    ParticleSystem myParticles;
    Rigidbody2D myBody;

    const float LIFETIME = 7f; // destruction after this number of seconds
    bool isExplosionGameobjectActive = false;
    // countdown before explosion
    float explosionDelay = 0f;

    void Start()
    {
        myParticles = GetComponent<ParticleSystem>();
        myBody = GetComponent<Rigidbody2D>();

        // set myParticles trigger to water shape so they destroy on contact with water
        Component watershapeComponent = FindFirstObjectByType<SpringsController>();
        myParticles.trigger.AddCollider(watershapeComponent);

        // set destruction eventually at lifetime
        Destroy(gameObject, LIFETIME);
    }

    void Update()
    {
        var deltatime = Time.deltaTime;

        if (explosionDelay > 0f)
        {
            explosionDelay -= deltatime;
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

        bool canCauseExplosion = (
            collision.gameObject.layer == LayerMask.NameToLayer("Water") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Icebergs")
        );

        if (canCauseExplosion && !isExplosionGameobjectActive)
        {
            //activate the explosion game object
            // if iceberg
            if (collision.gameObject.layer == LayerMask.NameToLayer("Icebergs"))
            {
                // set current particle system to iceberg explosion
                currentExplosion = explosionOnIcebergParticleSystem;

                // cause damages
                var icebergLogic = collision.GetComponent<IcebergLogic>();
                icebergLogic.GetDamage(1);
            }
            else
            {
                // if it is water or something else, set particle to normal explosion
                currentExplosion = explosionParticleSystem;
            }

            // activate explosion particles
            currentExplosion.gameObject.SetActive(true);
            isExplosionGameobjectActive = true;

            // set missile particle system shape arc to 360
            var particlesShape = myParticles.shape;
            particlesShape.arc = 360f;

            // set missile's alpha to 0
            var transparentColor = new Color(0f, 0f, 0f, 0f);
            GetComponent<SpriteRenderer>().color = transparentColor;

            // set missile velocity to zero
            myBody.linearVelocity = Vector2.zero;

            // start countdown of Xs
            explosionDelay = 2f;
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
