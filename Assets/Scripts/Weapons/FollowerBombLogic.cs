using UnityEngine;

public class FollowerBombLogic : MonoBehaviour
{
    // particle systems fields
    [SerializeField] ParticleSystem normalExplosion;
    [SerializeField] ParticleSystem onIcebergExplosion;

    ParticleSystem currentExplosion, myParticles;


    // boat ref
    GameObject Boat;
    Rigidbody2D myBody;

    float timeBeforeStartingFollowing = .5f;
    const float timeBetweenFollowUpdate = .3f; // follow the boat each X seconds
    float countdown = 0f;
    float xVelocity = 0f;
    bool isExplosionActive = false;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.linearVelocityY = .3f;

        myParticles = GetComponent<ParticleSystem>();
        // set myParticles trigger to water shape so they destroy on contact with water
        Component watershapeComponent = FindFirstObjectByType<SpringsController>();
        myParticles.trigger.AddCollider(watershapeComponent);

        // get the boat object by it unique script "BoatLogic"
        Boat = FindFirstObjectByType<BoatLogic>().gameObject;
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if (isExplosionActive && countdown <= 0f)
        {
            DestroyAfterExplosionEffect();
        }
    }

    void FixedUpdate()
    {
        if (isExplosionActive)
        {
            return;
        }

        if (timeBeforeStartingFollowing > 0f)
        {
            timeBeforeStartingFollowing -= Time.deltaTime;
        }
        else
        {
            FollowBoat();

        }
    }

    void FollowBoat()
    {
        if (countdown <= 0f)
        {
            var xDifference = Boat.transform.position.x - transform.position.x;
            if (Mathf.Abs(xDifference) > Mathf.Epsilon)
            {
                xVelocity = (xDifference / Mathf.Abs(xDifference)) * 2f;

            }

            var tolerance = .2f;
            countdown = Random.Range(timeBetweenFollowUpdate - tolerance, timeBetweenFollowUpdate + tolerance);
        }

        // lerp velocity to avoid abrupt change in direction
        myBody.linearVelocityX = Mathf.Lerp(myBody.linearVelocityX, xVelocity, .07f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool mustExplode = (
            other.gameObject.layer == LayerMask.NameToLayer("Boat") ||
            other.gameObject.layer == LayerMask.NameToLayer("Icebergs") ||
            other.gameObject.layer == LayerMask.NameToLayer("Missiles")
        );

        if (mustExplode && !isExplosionActive)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Icebergs"))
            {
                // set current particle system to iceberg explosion
                currentExplosion = onIcebergExplosion;

                // cause damages
                var icebergLogic = other.GetComponent<IcebergLogic>();
                icebergLogic.GetDamage(1);
            }
            else
            {
                // if it is water or something else, set particle to normal explosion
                currentExplosion = normalExplosion;

                // inflige damage if Boat
                if (other.gameObject.layer == LayerMask.NameToLayer("Boat"))
                {
                    float damageAmount = .1f;

                    other.gameObject.GetComponent<BoatLogic>().GetDamage(damageAmount);
                }
            }

            // activate explosion particles
            currentExplosion.gameObject.SetActive(true);
            isExplosionActive = true;

            // set missile's alpha to 0
            var transparentColor = new Color(0f, 0f, 0f, 0f);
            GetComponent<SpriteRenderer>().color = transparentColor;

            // set missile velocity to zero
            myBody.linearVelocity = Vector2.zero;

            // set countdown to Xs
            countdown = 2f;
        }
    }

    void DestroyAfterExplosionEffect()
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
