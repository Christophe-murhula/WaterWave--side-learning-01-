using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeGUILogic : MonoBehaviour
{
    [SerializeField] TMP_Text gameTitle;
    [SerializeField] Button playBtn, SettingsBtn, quitBtn;
    bool active = true;

    private void FixedUpdate()
    {
        if (!active)
        {
            if (!IsFadingOutOver())
            {
                FadeOut();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void FadeOut()
    {
        float alphaLerp = Mathf.Lerp(gameTitle.color.a, 0f, 0.05f);

        // shade game title
        gameTitle.alpha = alphaLerp;

        // shade play button and its text
        ShadeButton(playBtn, alphaLerp);
        // shade play button and its text
        ShadeButton(SettingsBtn, alphaLerp);
        // shade play button and its text
        ShadeButton(quitBtn, alphaLerp);
    }

    void ShadeButton(Button btn, float alpha)
    {
        Color color = btn.image.color;

        // shade the button
        btn.image.color = new Color(color.r, color.g, color.b, Mathf.Min(alpha, color.a));

        // shade it text
        btn.GetComponentInChildren<TMP_Text>().alpha = alpha;
    }

    public bool IsFadingOutOver()
    {
        return gameTitle.color.a <= Mathf.Epsilon;
    }

    public void Deactivate()
    {
        active = false;

        // deactivate the buttons
        playBtn.interactable = false;
        SettingsBtn.interactable = false;
        quitBtn.interactable = false;
    }
}
