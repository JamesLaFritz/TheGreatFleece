using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CutSceneGameEventListener m_beginningCutscene;
    [SerializeField] private CutSceneGameEventListener m_gameOverCutscene;
    [SerializeField] private CutSceneGameEventListener m_sleepingGuardCutscene;
    [SerializeField] private CutSceneGameEventListener m_completeLevelCutscene;

    [SerializeField] private GameObject keyCard;

    private bool m_hasKeyCard;

    private void Start()
    {
        if (m_beginningCutscene != null) m_beginningCutscene.Init();
        if (m_gameOverCutscene != null) m_gameOverCutscene.Init();
        if (m_sleepingGuardCutscene != null) m_sleepingGuardCutscene.Init();
        if (m_completeLevelCutscene != null) m_completeLevelCutscene.Init();
    }

    private void OnEnable()
    {
        if (m_beginningCutscene != null) m_beginningCutscene.OnEnable(OnBeginningCutsceneEvent);
        if (m_gameOverCutscene != null) m_gameOverCutscene.OnEnable(OnGameOverCutsceneEvent);
        if (m_sleepingGuardCutscene != null) m_sleepingGuardCutscene.OnEnable(OnSleepingGuardCutsceneEvent);
        if (m_completeLevelCutscene != null) m_completeLevelCutscene.OnEnable(OnCompleteLevelCutsceneEvent);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (m_beginningCutscene != null) m_beginningCutscene.OnDisable();
        if (m_gameOverCutscene != null) m_gameOverCutscene.OnDisable();
        if (m_sleepingGuardCutscene != null) m_sleepingGuardCutscene.OnDisable();
        if (m_completeLevelCutscene != null) m_completeLevelCutscene.OnDisable();
    }

    private void OnBeginningCutsceneEvent()
    {
        Debug.Assert(m_beginningCutscene != null, nameof(m_beginningCutscene) + " != null");
        StartCoroutine(m_beginningCutscene.PlayCutscene());
        m_hasKeyCard = false;
    }

    private void OnGameOverCutsceneEvent()
    {
        Debug.Assert(m_gameOverCutscene != null, nameof(m_gameOverCutscene) + " != null");
        StartCoroutine(m_gameOverCutscene.PlayCutscene());
        m_hasKeyCard = false;
    }

    private void OnSleepingGuardCutsceneEvent()
    {
        Debug.Assert(m_sleepingGuardCutscene != null, nameof(m_sleepingGuardCutscene) + " != null");
        StartCoroutine(m_sleepingGuardCutscene.PlayCutscene());
        if (keyCard != null) keyCard.SetActive(false);
        m_hasKeyCard = true;
    }

    private void OnCompleteLevelCutsceneEvent()
    {
        if (!m_hasKeyCard) return;
        Debug.Assert(m_completeLevelCutscene != null, nameof(m_completeLevelCutscene) + " != null");
        StartCoroutine(m_completeLevelCutscene.PlayCutscene());
    }
}
