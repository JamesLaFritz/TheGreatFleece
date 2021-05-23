using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public enum PatrolType
{
    PingPong,
    Loop,
    OneTime
}

public class WaypointMovement : NavMeshAgentMovement
{
    [SerializeField] private PatrolType m_type = PatrolType.Loop;
    [SerializeField] private List<Transform> m_waypoints = new List<Transform>();

    private int m_destinationPointIndex;
    private int m_changeIndexBy = 1;

    #region Overrides of NavMeshAgentMovement

    /// <inheritdoc />
    protected override void SetDestination()
    {
        Debug.Assert(m_waypoints != null, nameof(m_waypoints) + " != null");
        if (m_waypoints.Count == 0 || // If the List is empty or
            m_destinationPointIndex < 0 || // If the index is less then 0 or
            m_destinationPointIndex >= m_waypoints.Count)// If the Index is greater then
            return;

        Debug.Assert(m_waypoints != null, nameof(m_waypoints) + " != null");
        Vector3? position = m_waypoints[m_destinationPointIndex]?.position;
        if (position != null)
            destination = (Vector3) position;
    }

    /// <inheritdoc />
    protected override void StopMovement()
    {
        Debug.Assert(m_waypoints != null, nameof(m_waypoints) + " != null");
        if (m_waypoints.Count <= 0)
            return;

        switch (m_type)
        {
            case PatrolType.PingPong:
                // Choose the next point in the array
                m_destinationPointIndex += m_changeIndexBy;
                // If the end of the list has been reached we need to start moving in the opposite direction
                if (m_destinationPointIndex >= m_waypoints.Count || m_destinationPointIndex < 0)
                {
                    m_changeIndexBy *= -1;

                    // Clamp the destination index within the bounds of the list
                    m_destinationPointIndex = Mathf.Clamp(m_destinationPointIndex, 1, m_waypoints.Count - 2);
                }
                
                break;
            case PatrolType.Loop:
                // Choose the next point in the array as the destination,
                // cycling to the start if necessary.
                m_destinationPointIndex = (m_destinationPointIndex + 1) % m_waypoints.Count;
                break;
            case PatrolType.OneTime:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
