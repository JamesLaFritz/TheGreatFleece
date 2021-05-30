using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Scene] private int GameSceneToLoad = 1;

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
        Application.OpenURL("https://JamesLaFritz.com");
        #else
        Application.Quit();
        #endif
    }
}
