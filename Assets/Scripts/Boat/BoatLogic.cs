using Unity.VisualScripting;
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

    float VIGNETTE_DEFAULT_INTENSITY;
    float VIGNETTE_DEFAULT_Y_POS;
    float VIGNETTE_DEFAULT_SMOOTHNESS;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myParticles = GetComponent<ParticleSystem>();
        mySprite = GetComponent<SpriteRenderer>();

        // get the vignette volume
        vignetteVolume = FindFirstObjectByType<Volume>();

        VolumeProfile profile = vignetteVolume.profile;
        profile.TryGet(out vignette);

        VIGNETTE_DEFAULT_INTENSITY = vignette.intensity.value;
        VIGNETTE_DEFAULT_Y_POS = vignette.center.value.y;
        VIGNETTE_DEFAULT_SMOOTHNESS = vignette.smoothness.value;
    }

    void Update()
    {
        if (vignette == null)
        {
            return;
        }
        VignetteControl();
    }

    public void GetDamage(float damageAmount)
    {
        if (IsAlive() && canGetDamage)
        {
            canGetDamage = false;

            HP -= damageAmount;

            Invoke("RestoreVulnerability", 1f);

            // a slight push back
            myBody.AddForceX(1f, ForceMode2D.Impulse);

            myParticles.Play();

            // change vignette
            vignette.intensity.value += .2f;
            vignette.center.value = new Vector2(vignette.center.value.x, .6f);
            vignette.smoothness.value += .45f;

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

    /// <summary>
    /// Modified variables: Intensity, Y-offet, Smoothness
    /// </summary>
    void VignetteControl()
    {
        if (vignette.smoothness.value > VIGNETTE_DEFAULT_SMOOTHNESS)
        {
            float othersLerp = .03f, smoothnessLerp = .01f;

            // lerp intensity and y-offset
            var intensity = Mathf.Lerp(vignette.intensity.value, VIGNETTE_DEFAULT_INTENSITY, othersLerp);
            var yOffset = Mathf.Lerp(vignette.center.value.y, VIGNETTE_DEFAULT_Y_POS, othersLerp);

            vignette.intensity.value = intensity;
            vignette.center.value = new Vector2(vignette.center.value.x, yOffset);

            // lerp the smoothness
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, VIGNETTE_DEFAULT_SMOOTHNESS, smoothnessLerp);
        }
    }
}
