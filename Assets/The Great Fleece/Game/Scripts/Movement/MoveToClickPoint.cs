using UnityEngine;

public class MoveToClickPoint : NavMeshAgentMovement
{
    [SerializeField] private GameObject m_clickedVisualPrefab;
    private bool m_hasClickedPrefab;
    private GameObject m_clickedVisualGameObject;
    private bool m_hasClickedVisualGameObject;

    #region Overrides of NavMeshAgentMovement

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
