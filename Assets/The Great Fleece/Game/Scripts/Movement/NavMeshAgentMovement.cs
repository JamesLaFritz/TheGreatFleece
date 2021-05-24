using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NavMeshAgentMovement : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    private bool m_hasAnimator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    protected NavMeshAgent agent;

    protected Vector3 destination;

    private Vector3 m_currentDestination;

    /// <summary>
    /// Gets the NavMesh Agent Component.
    /// Sets the destination to the transforms position.
    /// Checks to see if there is an animator.
    /// </summary>
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent is null)
        {
            Debug.LogException(
                new System.ArgumentNullException(
                    $"Something went horriably wrong. Unable to get the NavMesh Agent Component"), gameObject);
            enabled = false;
        }

        m_currentDestination = destination = transform.position;

        m_hasAnimator = m_animator != null;
        if (m_animator) return;

        m_animator = GetComponent<Animator>();
        m_hasAnimator = m_animator != null;
        if (m_animator) return;

        m_animator = GetComponentInChildren<Animator>();
        m_hasAnimator = m_animator != null;
        if (m_animator) return;

        Debug.LogWarning("Animator has not been set and I could not find one on this Object or in it's" +
                         " children was this intentional. This is needed in order to set the Animation" +
                         " Parameters.", gameObject);
    }

    /// <summary>
    /// Call Set Destination.
    /// If the destination Changes sets the agent's destination to the new destination.
    /// IF the agent.remainingDistance less then equal to 0 Call Stop Movement.
    /// If there is an animator set the ISWalking Parameter to agent.remainingDistance > 0
    /// </summary>
    protected virtual void Update()
    {
        SetDestination();

        Debug.Assert(agent != null, nameof(agent) + " != null");

        if (m_currentDestination != destination)
        {
            m_currentDestination = destination;
            agent.SetDestination(destination);
        }

        //Debug.Log($"{agent.remainingDistance} : {agent.stoppingDistance}");
        if (!agent.pathPending && agent.remainingDistance <= 0)
        {
            StopMovement();
        }

        if (!m_hasAnimator) return;
        // ReSharper disable PossibleNullReferenceException
        m_animator.SetBool(IsWalking, agent.remainingDistance > 0);
        // ReSharper restore PossibleNullReferenceException
    }

    /// <summary>
    /// Set the destination that the agent should move to.
    /// The agents destination gets set in the update method if the destination variable changes.
    /// </summary>
    protected abstract void SetDestination();

    /// <summary>
    /// This Method is Called From within the Update Method when the agent.remainingDistance less then 0
    /// </summary>
    protected abstract void StopMovement();
}
