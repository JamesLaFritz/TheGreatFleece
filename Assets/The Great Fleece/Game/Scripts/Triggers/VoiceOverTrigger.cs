using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    private bool m_hasBeenTriggered;

    [SerializeField] [Tag] private string m_triggerTag = "Player";

    private bool m_hasTags;

    [SerializeField] AudioClip m_voiceOverClip;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{name} Colliding with {other.name} with tag {other.name}");

        if (m_hasBeenTriggered) return;

        Debug.Assert(other != null, nameof(other) + " != null");
        if (!other.CompareTag(m_triggerTag)) return;

        Debug.Assert(AudioManager.Instance != null, "AudioManager.Instance != null");
        AudioManager.Instance.PlayVoiceOverClip(m_voiceOverClip);

        m_hasBeenTriggered = true;
    }
}
