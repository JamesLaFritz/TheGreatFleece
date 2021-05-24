using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class StartLevel : MonoBehaviour
{
    [SerializeField] private CodedGameEventListener m_beginningEventListener;

    [SerializeField] private GameObject m_beggingCutScene;
    private bool m_hasCutScene;
    private PlayableDirector m_playableDirector;
    private bool m_hasPlayableDirector;

    private void OnEnable()
    {
        m_beginningEventListener?.OnEnable(OnBeginningEventRaised);
    }

    private void OnDisable()
    {
        m_beginningEventListener?.OnDisable();
    }

    private void Start()
    {
        m_hasCutScene = m_beggingCutScene != null;

        if (m_hasCutScene)
        {
            System.Diagnostics.Debug.Assert(m_beggingCutScene != null, nameof(m_beggingCutScene) + " != null");
            m_playableDirector = m_beggingCutScene.GetComponent<PlayableDirector>();
            m_hasPlayableDirector = m_playableDirector != null;
        }
    }

    private void OnBeginningEventRaised()
    {
        if (!m_hasCutScene) return;

        Debug.Assert(m_beggingCutScene != null, nameof(m_beggingCutScene) + " != null");
        m_beggingCutScene.SetActive(true);

        StartCoroutine(CutScenePlaying());
    }

    public void StartCutscene()
    {
        StartCoroutine(CutScenePlaying());
    }

    private IEnumerator CutScenePlaying()
    {
        if (!m_hasPlayableDirector) yield break;

        Debug.Assert(m_playableDirector != null, nameof(m_playableDirector) + " != null");
        yield return new WaitForSeconds((float)m_playableDirector.duration);

        Debug.Assert(m_beggingCutScene != null, nameof(m_beggingCutScene) + " != null");
        m_beggingCutScene.SetActive(false);
    }
}
