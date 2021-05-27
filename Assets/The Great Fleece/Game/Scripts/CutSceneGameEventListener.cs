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
        if (!m_hasCutScene && !m_hasPlayableDirector)
            Init();
        if (!m_hasCutScene || !m_hasPlayableDirector)
            yield break;

        // enable the cut scene
        System.Diagnostics.Debug.Assert(m_cutScene != null, nameof(m_cutScene) + " != null");
        m_cutScene.SetActive(true);

        // wait for the cut scene to be played
        System.Diagnostics.Debug.Assert(m_playableDirector != null, nameof(m_playableDirector) + " != null");
        yield return new WaitForSeconds((float) m_playableDirector.duration);

        // disable the cutscene
        m_cutScene.SetActive(false);
    }
}
