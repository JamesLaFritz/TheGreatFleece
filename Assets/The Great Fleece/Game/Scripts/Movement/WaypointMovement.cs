using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PatrolType
{
    PingPong,
    Loop,
    OneTime
}
public class WaypointMovement : NavMeshAgentMovement
{
    [SerializeField] private PatrolType m_type = PatrolType.Loop;

    [SerializeField] private float m_timeToWaitBetweenMovingMin = 2f;
    [SerializeField] private float m_timeToWaitBetweenMovingMax = 5f;
    private bool m_isWaiting;

    // A list of positions to set the destination to.
    [SerializeField] private List<Transform> m_waypoints = new List<Transform>();

    [SerializeField] private int m_startIndex = 0;

    // A way to track which position in the list we are currently at.
    private int m_destinationPointIndex;

    private int m_changeIndexBy = 1;

    #region Overrides of NavMeshAgentMovement

    /// <inheritdoc />
    protected override void Start()
    {
        base.Start();

        m_destinationPointIndex = m_startIndex;
    }

    /// <inheritdoc />
    protected override void SetDestination()
    {
        // set the destination to be the destination that we are currently at in the list
        Debug.Assert(m_waypoints != null, nameof(m_waypoints) + " != null");

        // If the List is empty or the current position is outside the list bounds
        if (m_waypoints.Count == 0 ||
            m_destinationPointIndex < 0 || // (If the index is less then 0 or
            m_destinationPointIndex >= m_waypoints.Count) // If the Index is greater then the number of items in the list)
        {
            return; // exit out of the method
        }

        // get the position that we are currently in the list
        Vector3? position = m_waypoints[m_destinationPointIndex]?.position;

        if (position != null)
        {
            // set the destination to the position
            destination = (Vector3) position;
        }
    }

    /// <inheritdoc />
    protected override void StopMovement()
    {
        // If the guard is waiting exit
        if (m_isWaiting) return;

        // Start the Set next destination routine
        StartCoroutine(SetNextDestination());
    }

    #endregion

    private IEnumerator SetNextDestination()
    {
        // We are starting the waiting set is waiting to true;
        m_isWaiting = true;

        // If we are here then the agent has reached it's destination.
        if (m_waypoints.Count <= 0)
        {
            m_isWaiting = false;
            yield break;
        }

        // Wait for wait time
        if (m_destinationPointIndex == 0 || m_destinationPointIndex == m_waypoints.Count - 1)
            yield return new WaitForSeconds(Random.Range(m_timeToWaitBetweenMovingMin, m_timeToWaitBetweenMovingMax));

        switch (m_type)
        {
            case PatrolType.PingPong:
                // Choose the next point in the array
                m_destinationPointIndex += m_changeIndexBy;

                // If the end of the list has been reached
                if (m_destinationPointIndex >= m_waypoints.Count || m_destinationPointIndex < 0)
                {
                    // I need to revers the direction that I am moving in the list.
                    m_changeIndexBy *= -1;
                    // Clamp the destination index within the bounds of the list
                    m_destinationPointIndex = Mathf.Clamp(m_destinationPointIndex, 1, m_waypoints.Count - 2);
                }

                break;
            case PatrolType.Loop:
                // Choose the next point in the list as the destination,
                // cycling to the start if necessary.
                m_destinationPointIndex = (m_destinationPointIndex + 1) % m_waypoints.Count;
                break;
            case PatrolType.OneTime:
                // Increase the value of the Index keeping it in bounds of the list.
                m_destinationPointIndex = math.min(m_destinationPointIndex++, m_waypoints.Count - 1);
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }

        // The guard is no longer waiting.
        m_isWaiting = false;
    }
}
