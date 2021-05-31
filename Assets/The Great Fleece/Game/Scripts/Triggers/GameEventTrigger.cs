using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField] private bool m_TriggerOnce;
    private bool m_hasGameEventBeenTriggered;

    [SerializeField] private GameEvent m_event;
    private bool m_hasGameEvent = true;

    [SerializeField] [Tag] private string[] m_triggerTags = new[] {"Player"};
    private bool m_hasTags;

    private void Start()
    {
        m_hasGameEvent = m_event != null;
        m_hasTags = m_triggerTags != null && m_triggerTags.Length > 0;

        if (!m_hasGameEvent)
        {
            Debug.LogWarning("No Game Event assigned, please assign one in the Inspector!", gameObject);
        }

        if (!m_hasTags)
        {
            Debug.LogWarning("No Tags assigned, please assign at least one in the Inspector!", gameObject);
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{name} Colliding with {other.name} with tag {other.name}");
        if (!m_hasGameEvent || !m_hasTags) return;

        if (m_TriggerOnce && m_hasGameEventBeenTriggered) return;

        Debug.Assert(m_triggerTags != null, nameof(m_triggerTags) + " != null");
        foreach (string triggerTag in m_triggerTags)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            if (!other.CompareTag(triggerTag)) continue;

            m_hasGameEventBeenTriggered = true;
            Debug.Assert(m_event != null, nameof(m_event) + " != null");
            //Debug.Log($"Raising Event {m_event.name}");
            m_event.Raise();
        }
    }
}
