using UnityEngine;

public class MoveToClickPoint : NavMeshAgentMovement
{
    [SerializeField] private GameObject goal;
    private bool m_hasGoal;
    private GameObject m_instantiatedGameObject;
    private bool m_hasInstantiatedGameObject;

    #region Overrides of NavMeshAgentMovement

    /// <inheritdoc />
    protected override void Start()
    {
        base.Start();

        m_hasGoal = goal != null;
    }

    /// <inheritdoc />
    protected override void Update()
    {
        base.Update();

        // ReSharper disable once PossibleNullReferenceException
        if (!m_hasInstantiatedGameObject || !(agent.remainingDistance < agent.radius)) return;
        m_hasInstantiatedGameObject = false;
        Destroy(m_instantiatedGameObject);
        m_instantiatedGameObject = null;
    }

    /// <inheritdoc />
    protected override void SetDestination()
    {
        if (!Input.GetButtonDown("Fire1")) return;

        // ReSharper disable once PossibleNullReferenceException
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100)) return;
        destination = hit.point;
        if (!m_hasGoal) return;
        if (m_hasInstantiatedGameObject)
        {
            m_hasInstantiatedGameObject = false;
            Destroy(m_instantiatedGameObject);
            m_instantiatedGameObject = null;
        }

        m_instantiatedGameObject = Instantiate(goal, destination, Quaternion.identity);
        m_hasInstantiatedGameObject = true;
    }

    #endregion
}
