using UnityEngine;

public class MoveTo : NavMeshAgentMovement
{
    [SerializeField] private Transform m_goal;

    /// <inheritdoc />
    protected override void SetDestination()
    {
        if (m_goal is null) return;
        destination = m_goal.position;
    }

    /// <inheritdoc />
    protected override void StopMovement() { }
}
