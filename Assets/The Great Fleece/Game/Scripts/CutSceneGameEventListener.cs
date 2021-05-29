using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CutSceneGameEventListener
{
    [SerializeField] private CodedGameEventListener m_cutsceneEventListener;

    [SerializeField] private GameObject m_cutScene;
    private bool m_hasCutScene;
    private PlayableDirector m_playableDirector;
    private bool m_hasPlayableDirector;

    [SerializeField] private AudioClip m_cutsceneBackgroundMusic;

    public void OnEnable(System.Action onCutsceneEventRaised)
    {
        m_cutsceneEventListener?.OnEnable(onCutsceneEventRaised);
    }

    public void OnDisable()
    {
        m_cutsceneEventListener?.OnDisable();
    }

    public void Init()
    {
        m_hasCutScene = m_cutScene != null;

        if (m_hasCutScene)
        {
            Debug.Assert(m_cutScene != null, nameof(m_cutScene) + " != null");
            m_playableDirector = m_cutScene.GetComponent<PlayableDirector>();
            m_hasPlayableDirector = m_playableDirector != null;
        }
    }

    public IEnumerator PlayCutscene()
    {
        if (!HasCutSceneToPlay())
            Init();
        if (!HasCutSceneToPlay())
            yield break;

        // enable the cut scene
        Debug.Assert(m_cutScene != null, nameof(m_cutScene) + " != null");
        Debug.Assert(GameManager.Instance != null, "GameManager.Instance != null");
        Debug.Assert(AudioManager.Instance != null, "AudioManager.Instance != null");

        m_cutScene.SetActive(true);
        GameManager.Instance.CurrentCutscene = this;
        GameManager.Instance.IsCutscenePlaying = true;
        AudioManager.Instance.PlayBackgroundMusic(m_cutsceneBackgroundMusic);

        // wait for the cut scene to be played
        Debug.Assert(m_playableDirector != null, nameof(m_playableDirector) + " != null");
        yield return new WaitForSeconds((float) m_playableDirector.duration);

        StopPlaying();
    }

    public IEnumerator SkipCutScene()
    {
        Debug.Assert(GameManager.Instance != null, "GameManager.Instance != null");
        GameManager.Instance.IsCutscenePlaying = false;

        if (!HasCutSceneToPlay())
            yield break;

        Debug.Assert(m_playableDirector != null, nameof(m_playableDirector) + " != null");
        m_playableDirector.time = m_playableDirector.duration - 0.3f;

        yield return new WaitForSeconds(0.3f);

        StopPlaying();
    }

    private void StopPlaying()
    {
        if (!HasCutSceneToPlay())
            return;

        // disable the cutscene
        Debug.Assert(GameManager.Instance != null, "GameManager.Instance != null");
        Debug.Assert(AudioManager.Instance != null, "AudioManager.Instance != null");
        Debug.Assert(m_cutScene != null, nameof(m_cutScene) + " != null");

        GameManager.Instance.IsCutscenePlaying = false;
        AudioManager.Instance.PlayBackgroundMusic();
        m_cutScene.SetActive(false);
        GameManager.Instance.CurrentCutscene = null;
    }

    private bool HasCutSceneToPlay()
    {
        return m_hasCutScene && m_hasPlayableDirector;
    }
}
