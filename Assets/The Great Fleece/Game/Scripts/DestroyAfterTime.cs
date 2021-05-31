using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float m_timeToLive = 1.5f;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, m_timeToLive);
    }
}
