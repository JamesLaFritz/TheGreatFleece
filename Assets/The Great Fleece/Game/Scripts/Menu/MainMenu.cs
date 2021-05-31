using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Scene] private int GameSceneToLoad = 2;

    [SerializeField, Scene] private int LoadingScreenBuildIndex = 1;

    public void LoadScene()
    {
        LoadLevel.sceneToLoadBuildIndex = GameSceneToLoad;
        SceneManager.LoadScene(LoadingScreenBuildIndex);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
        Application.OpenURL("https://JamesLaFritz.com");
        #endif

        Application.Quit();
    }
}
