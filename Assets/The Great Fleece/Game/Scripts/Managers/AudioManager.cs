using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_voiceOverAudioSource;
    private bool m_hasVoiceOverSource;
    [SerializeField] private AudioSource m_backgroundAudioSource;
    private bool m_hasBackgroundSource;
    [SerializeField] private AudioClip m_defaultBackgroundMusic;
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The Audio Manager is null!!!");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        m_hasBackgroundSource = m_backgroundAudioSource != null;
        m_hasVoiceOverSource = m_voiceOverAudioSource != null;
    }

    public void PlayVoiceOverClip(AudioClip clip)
    {
        if (!m_hasVoiceOverSource) return;
        Debug.Assert(m_voiceOverAudioSource != null, nameof(m_voiceOverAudioSource) + " != null");
        m_voiceOverAudioSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(AudioClip clip = null)
    {
        if (!m_hasBackgroundSource) return;

        Debug.Assert(m_backgroundAudioSource != null, nameof(m_backgroundAudioSource) + " != null");

        if (m_backgroundAudioSource.isPlaying)
            m_backgroundAudioSource.Stop();

        if (clip == null)
            clip = m_defaultBackgroundMusic;
        m_backgroundAudioSource.clip = clip;
        m_backgroundAudioSource.loop = true;
        m_backgroundAudioSource.Play();
    }
}
