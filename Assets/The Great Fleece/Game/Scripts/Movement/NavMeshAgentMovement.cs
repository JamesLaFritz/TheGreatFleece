using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NavMeshAgentMovement : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected Vector3 destination;

    private Vector3 m_currentDestination;

    /// <summary>
    /// Gets the NavMesh Agent Component.
    /// Sets the destination to the transforms position.
    /// </summary>
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent is null)
        {
            Debug.LogException(
                new System.ArgumentNullException(
                    $"Something went horriably wrong. Unable to get the NavMesh Agent Component"), gameObject);
        }

        m_currentDestination = destination = transform.position;
    }

    /// <summary>
    /// Call Set Destination
    /// If the destination Changes sets the agent's destination to the new destination.
    /// </summary>
    protected virtual void Update()
    {
        SetDestination();

        if (m_currentDestination == destination) return;

        m_currentDestination = destination;

        Debug.Assert(agent != null, nameof(agent) + " != null");
        agent.SetDestination(destination);
    }

    /// <summary>
    /// Set the destination that the agent should move to.
    /// The agents destination gets set in the update method if the destination variable changes.
    /// </summary>
    protected abstract void SetDestination();
}
