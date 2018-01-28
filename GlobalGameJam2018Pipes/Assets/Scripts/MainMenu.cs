
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnSingleplayerButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnMultiplayerButtonClicked()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
