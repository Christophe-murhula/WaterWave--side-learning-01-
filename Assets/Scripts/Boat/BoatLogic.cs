using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BoatLogic : MonoBehaviour
{
    Rigidbody2D myBody;
    ParticleSystem myParticles;
    SpriteRenderer mySprite;

    Vignette vignette;
    Volume vignetteVolume;

    float HP = 1f;
    bool canGetDamage = true;
    float VIGNETTE_DEFAULT_SMOOTHNESS;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myParticles = GetComponent<ParticleSystem>();
        mySprite = GetComponent<SpriteRenderer>();

        // get the vignette volume
        Volume[] volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None);
        foreach (Volume v in volumes)
        {
            if (v.priority == 2)
            {
                vignetteVolume = v;

                VolumeProfile profile = v.profile;
                profile.TryGet(out vignette);

                VIGNETTE_DEFAULT_SMOOTHNESS = vignette.smoothness.value;
            }
        }
    }

    void Update()
    {
        if (vignette == null)
        {
            return;
        }
        VignetteControl();
    }

    void FixedUpdate()
    {
        if (!canGetDamage && mySprite.color.a == 255f)
        {
            mySprite.color = new Color(255f, 255f, 255f, 100f);
        }
        else if (canGetDamage && mySprite.color.a == 100f)
        {
            mySprite.color = new Color(255f, 255f, 255f, 255f);
        }
    }

    public void GetDamage(float damageAmount, Vector2 contactPoint)
    {
        if (IsAlive() && canGetDamage)
        {
            canGetDamage = false;

            HP -= damageAmount;

            Invoke("RestoreVulnerability", 1f);

            // a slight push back
            myBody.AddForceX(1f, ForceMode2D.Impulse);

            myParticles.Play();

            // activate vignette
            vignetteVolume.priority = 2;
            vignette.active = true;
            vignette.smoothness.value += .4f;

            if (!IsAlive())
            {
                // game over
                print("gameover");
            }
        }
    }

    bool IsAlive()
    {
        return HP > 0F;
    }

    void RestoreVulnerability()
    {
        canGetDamage = true;
    }

    void VignetteControl()
    {
        if (vignette.smoothness.value != VIGNETTE_DEFAULT_SMOOTHNESS && vignette.active)
        {
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0f, .05f);
        }
        if (vignette.smoothness.value <= VIGNETTE_DEFAULT_SMOOTHNESS)
        {
            if (vignette.active)
            {
                vignetteVolume.priority = 0;
                vignette.active = false;
                vignette.smoothness.value = VIGNETTE_DEFAULT_SMOOTHNESS;
            }
        }
    }
}
