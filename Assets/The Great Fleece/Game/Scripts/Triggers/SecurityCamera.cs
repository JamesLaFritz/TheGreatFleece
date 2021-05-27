using System.Collections;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private GameEvent m_event;
    private bool m_hasGameEvent = true;

    [SerializeField] [Tag] private string[] m_triggerTags = new[] {"Player"};
    private bool m_hasTags;

    private Animator m_animator;
    private bool m_hasAnimator;

    private MeshRenderer m_render;
    private bool m_hasMeshRender;
    private MaterialPropertyBlock m_propertyBlock;
    private static readonly int PropertyColor = Shader.PropertyToID("_TintColor");
    [SerializeField] private Color m_alertColor = new Color(0.6f, 0.1f, 0.1f, 0.04f);

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

        m_render = GetComponent<MeshRenderer>();
        m_hasMeshRender = m_render != null;
        m_propertyBlock = new MaterialPropertyBlock();

        m_animator = GetComponentInParent<Animator>();
        m_hasAnimator = m_animator != null;
        if (m_hasAnimator) return;
        m_animator = GetComponent<Animator>();
        m_hasAnimator = m_animator != null;
        if (m_hasAnimator) return;
        m_animator = GetComponentInChildren<Animator>();
        m_hasAnimator = m_animator != null;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (!m_hasGameEvent || !m_hasTags) yield break;

        Debug.Assert(m_triggerTags != null, nameof(m_triggerTags) + " != null");
        foreach (string triggerTag in m_triggerTags)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            if (!other.CompareTag(triggerTag)) continue;

            if (m_hasAnimator)
            {
                Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
                m_animator.enabled = false;
            }

            if (m_hasMeshRender)
            {
                Debug.Assert(m_render != null, nameof(m_render) + " != null");
                Debug.Assert(m_propertyBlock != null, nameof(m_propertyBlock) + " != null");
                m_render.GetPropertyBlock(m_propertyBlock);
                m_propertyBlock.SetColor(PropertyColor, m_alertColor);
                m_render.SetPropertyBlock(m_propertyBlock);
            }

            Debug.Assert(m_event != null, nameof(m_event) + " != null");
            yield return new WaitForSeconds(5f);
            m_event.Raise();

            break;
        }
    }
}
