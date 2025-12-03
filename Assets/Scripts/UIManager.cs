using UnityEngine;

public class UIManager : MonoBehaviour
{
    // GUI-childs references
    [SerializeField] GameObject HomeGUI;
    // [SerializeField] GameObject PauseGUI;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void Play()
    {
        // set timescale to 1 and deactivate the HomeGUI
        Time.timeScale = 1f;

        HomeGUI.SetActive(false);
    }

    public void OnPause()
    {
        // set timescale to 1 and activate the PauseGUI
        Time.timeScale = 1f;

        // PauseGUI.SetActive(true);
    }

    bool IsGameRunning()
    {
        return Time.timeScale != 0f;
    }
}
