using UnityEngine;

public class WaterDropsLogic : MonoBehaviour
{
    [SerializeField] GameObject OneDrop;
    [SerializeField] Transform DropsOrigin;

    ushort numberOfDropsLeft = 0; // recharge everytime the boat is damaged
    const float longestDelayBetweenDrop = 3f; // 3 seconds
    float currentCountdown = 0f;

    private void Update()
    {
        Drop();
    }

    void Drop()
    {
        if (currentCountdown > 0f)
        {
            currentCountdown -= Time.deltaTime;
        }
        else
        {
            if (numberOfDropsLeft == 0) { return; }

            // set the current countdown to longestDelay / number of drops left
            currentCountdown = longestDelayBetweenDrop / numberOfDropsLeft;

            // create the drop
            var drop = Instantiate(OneDrop, DropsOrigin.position, DropsOrigin.rotation, parent: transform);
            var scale = 1f + (1f - transform.localScale.x);
            drop.transform.localScale = new Vector3(scale, scale, scale);

            // decrement the drops count
            numberOfDropsLeft -= 1;
        }
    }

    public void RechargeDrops(ushort quantity)
    {
        numberOfDropsLeft = quantity;
    }

}
