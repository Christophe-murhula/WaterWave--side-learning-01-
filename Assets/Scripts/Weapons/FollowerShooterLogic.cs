using UnityEngine;

public class FollowerShooterLogic : MonoBehaviour
{
    [SerializeField] GameObject followerBomb;

    BoxCollider2D myCollider;

    const float minTimeBetweenShoot = 4.5f, maxTimeBetweenShoot = 7.5f;
    const float vulnerabilityTime = 1.5f;

    float vulnerabilityCountdown = 0f, nextShootCountdown = 0f;

    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    // LOGIC: open (vulnerable), shoot once or twice, close (not vulnerable anymore), random choose next shooting time
    private void FixedUpdate()
    {
        if (!isOnScreen())
        {
            return;
        }

        var deltatime = Time.deltaTime;


        if (IsVulnerable()) // if we are vulnerable
        {
            // countdown vulnerability
            vulnerabilityCountdown -= deltatime;

            // choose the next shot time if not yet
            if (nextShootCountdown <= 0f)
            {
                nextShootCountdown = Random.Range(minTimeBetweenShoot, maxTimeBetweenShoot);
            }
        }
        else // i.e. if we are not vulnerable (closed)
        {
            // countdown for next opening
            if (nextShootCountdown > 0f)
            {
                nextShootCountdown -= deltatime;
            }
            else
            {
                // become vulnerable (by setting the vulnerability countdown)
                vulnerabilityCountdown = vulnerabilityTime;

                // invoke shoots (for two shoots)
                Invoke("Shoot", 0.35f);
                Invoke("Shoot", 0.85f);
                Invoke("Shoot", 1.35f);
            }
        }
    }

    void Shoot()
    {
        // generate the bomb at top of this shooter
        var pos = new Vector3(transform.position.x, myCollider.bounds.max.y, transform.position.z);
        var bomb = Instantiate(followerBomb, pos, transform.rotation);
    }

    bool isOnScreen()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return false;

        // convert Viewport Edges to World Space
        float zDistance = transform.position.z - mainCamera.transform.position.z;
        Vector3 screenLeftWorld = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 screenRightWorld = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        return (transform.position.x > screenLeftWorld.x) && (transform.position.x < screenRightWorld.x);
    }

    bool IsVulnerable()
    {
        return vulnerabilityCountdown > 0f;
    }
}
