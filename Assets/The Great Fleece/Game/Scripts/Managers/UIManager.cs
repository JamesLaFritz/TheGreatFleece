using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The Audio Manager is null!!!");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
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
