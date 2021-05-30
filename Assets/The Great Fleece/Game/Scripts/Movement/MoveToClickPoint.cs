using UnityEngine;

public class MoveToClickPoint : NavMeshAgentMovement
{
    [SerializeField] private GameObject m_clickedVisualPrefab;
    private bool m_hasClickedPrefab;
    private GameObject m_clickedVisualGameObject;
    private bool m_hasClickedVisualGameObject;

    [SerializeField] private CodedGameEventListener[] cutsceneEvents;

    #region Overrides of NavMeshAgentMovement

    private void OnEnable()
    {
        if (cutsceneEvents == null || cutsceneEvents.Length < 1) return;
        Debug.Assert(cutsceneEvents != null, nameof(cutsceneEvents) + " != null");
        foreach (CodedGameEventListener t in cutsceneEvents)
        {
            t?.OnEnable(StopMovement);
        }
    }

    private void OnDisable()
    {
        if (cutsceneEvents == null || cutsceneEvents.Length < 1) return;
        Debug.Assert(cutsceneEvents != null, nameof(cutsceneEvents) + " != null");
        foreach (CodedGameEventListener t in cutsceneEvents)
        {
            t?.OnDisable();
        }
    }

    /// <inheritdoc />
    protected override void Start()
    {
        base.Start();

        m_hasClickedPrefab = m_clickedVisualPrefab != null;
    }

    /// <inheritdoc />
    protected override void SetDestination()
    {
        if (!Input.GetButtonDown("Fire1")) return;

        // ReSharper disable once PossibleNullReferenceException
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100)) return;
        destination = hit.point;

        if (!m_hasClickedPrefab) return;
        if (m_hasClickedVisualGameObject)
        {
            m_hasClickedVisualGameObject = false;
            Destroy(m_clickedVisualGameObject);
            m_clickedVisualGameObject = null;
        }

        m_clickedVisualGameObject = Instantiate(m_clickedVisualPrefab, destination, Quaternion.identity);
        m_hasClickedVisualGameObject = true;
    }

    protected override void StopMovement()
    {
        if (!m_hasClickedVisualGameObject) return;
        m_hasClickedVisualGameObject = false;
        Destroy(m_clickedVisualGameObject);
        m_clickedVisualGameObject = null;
    }

    #endregion
}
