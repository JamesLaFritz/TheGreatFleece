using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgentMovement), typeof(NavMeshAgent))]
public class MoveToCoinTossPosition : MonoBehaviour
{
    [SerializeField] private float m_waitAtCoinTime = 2f;
    [SerializeField] CodedGameEventListener m_coinTossedGameEventListener;
    [SerializeField] private Vector3Variable m_coinTossedPosition;

    [SerializeField] private Animator m_animator;
    private bool m_hasAnimator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private NavMeshAgentMovement m_originalNavMeshAgentMovement;
    private bool m_hasOriginalNavMeshAgentMovement;

    private NavMeshAgent m_agent;

    private void OnEnable()
    {
        m_coinTossedGameEventListener?.OnEnable(OnCoinTossedRaised);
    }

    private void OnDisable()
    {
        m_coinTossedGameEventListener?.OnDisable();
    }

    private void Start()
    {
        if (m_coinTossedPosition == null)
        {
            Debug.LogException(
                new System.ArgumentNullException(
                    $"Coin Tossed Position required for this to work"), gameObject);
            enabled = false;
        }

        if (m_coinTossedGameEventListener == null)
        {
            Debug.LogException(
                new System.ArgumentNullException(
                    $"Coin Tossed Game Event Listener required for this to work"), gameObject);
            enabled = false;
        }

        m_agent = GetComponent<NavMeshAgent>();
        if (m_agent is null)
        {
            Debug.LogException(
                new System.ArgumentNullException(
                    $"Something went horriably wrong. Unable to get the NavMesh Agent Component"), gameObject);
            enabled = false;
        }

        m_originalNavMeshAgentMovement = GetComponent<NavMeshAgentMovement>();
        m_hasOriginalNavMeshAgentMovement = m_originalNavMeshAgentMovement != null;

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

    private void Update()
    {
        if (!m_hasAnimator) return;
        // ReSharper disable PossibleNullReferenceException
        m_animator.SetBool(IsWalking, !m_agent.pathPending && m_agent.remainingDistance > 0);
        // ReSharper restore PossibleNullReferenceException
    }

    private void OnCoinTossedRaised()
    {
        StartCoroutine(MoveAgentToDestination());
    }

    private IEnumerator MoveAgentToDestination()
    {
        if (m_hasOriginalNavMeshAgentMovement)
        {
            Debug.Assert(m_originalNavMeshAgentMovement != null, nameof(m_originalNavMeshAgentMovement) + " != null");
            m_originalNavMeshAgentMovement.enabled = false;
        }

        Debug.Assert(m_coinTossedPosition != null, nameof(m_coinTossedPosition) + " != null");
        Debug.Assert(m_agent != null, nameof(m_agent) + " != null");
        m_agent.SetDestination(m_coinTossedPosition.Value);

        while (m_agent.pathPending)
        {
            yield return null;
        }

        Debug.Assert(m_agent != null, nameof(m_agent) + " != null");
        while (m_agent.remainingDistance > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(m_waitAtCoinTime);


        if (!m_hasOriginalNavMeshAgentMovement) yield break;

        Debug.Assert(m_originalNavMeshAgentMovement != null, nameof(m_originalNavMeshAgentMovement) + " != null");
        m_originalNavMeshAgentMovement.enabled = true;
    }
}
