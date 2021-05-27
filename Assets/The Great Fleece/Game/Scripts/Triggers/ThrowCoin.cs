using UnityEngine;

public class ThrowCoin : MonoBehaviour
{
    [SerializeField] private GameObject m_coinPrefab;

    [SerializeField] private GameEvent m_coinTossed;

    [SerializeField] private Vector3Variable m_coinTossedPosition;

    [SerializeField] private Animator m_animator;
    private bool m_hasAnimator;
    private static readonly int Throw = Animator.StringToHash("Throw");

    private bool m_hasCoin = true;

    private void Start()
    {
        if (m_coinPrefab == null)
        {
            enabled = false;
        }

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

    private void Update()
    {
        //if right click
        if (!m_hasCoin || !Input.GetMouseButtonDown(1)) return;

        m_hasCoin = false;

        if (m_hasAnimator)
        {
            Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
            m_animator.SetTrigger(Throw);
        }

        // Get the mouse position
        // ReSharper disable once PossibleNullReferenceException
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100)) return;


        //instantiate the coin prefab at the mouse position
        Instantiate(m_coinPrefab, hit.point, Quaternion.identity);

        if (m_coinTossedPosition != null) m_coinTossedPosition.SetValue(hit.point);
        if (m_coinTossed != null) m_coinTossed.Raise();
    }
}
