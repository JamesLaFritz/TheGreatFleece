using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveTo : MonoBehaviour
{
    [SerializeField] private Transform m_goal;

    private void Start()
    {
        if (m_goal is null) return;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent is null) return;

        Debug.Assert(m_goal != null, nameof(m_goal) + " != null");
        agent.destination = m_goal.position;
    }
}
