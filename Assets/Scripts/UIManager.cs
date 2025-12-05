using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] HomeGUILogic homeGUILogic;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void Play()
    {
        // set time scale to 1
        Time.timeScale = 1f;
        // deactivate the home gui
        homeGUILogic.Deactivate();
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
