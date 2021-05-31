using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public static int sceneToLoadBuildIndex = 2;

    [SerializeField] Image m_progressBar;

    private void Start()
    {
        if (m_progressBar == null)
        {
            Image[] images = GetComponentsInChildren<Image>();

            if (images != null)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] == null || images[i].type != Image.Type.Filled) continue;
                    m_progressBar = images[i];
                    i = images.Length;
                }
            }

            if (m_progressBar == null)
                Debug.LogException(new System.NullReferenceException($"{gameObject.name}" +
                                                                     " Load Level Component Requires an Image" +
                                                                     " to use as the progress bar"));
        }

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        // Load Scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoadBuildIndex);
        asyncOperation.allowSceneActivation = false;

        // while the Game is still loading
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (asyncOperation is {isDone: false})
        {
            Debug.Assert(m_progressBar != null, nameof(m_progressBar) + " != null");
            m_progressBar.fillAmount = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Scene is fully Loaded");
                yield return new WaitForSeconds(0.5f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}