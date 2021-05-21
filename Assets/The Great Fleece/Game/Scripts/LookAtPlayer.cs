using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // variable to hold the object you want to look at
    [SerializeField] private Transform m_target;

    private void Start()
    {
        if (m_target != null) return;
        Debug.LogError("Target has not been set please set one in the Inspector." +
                       " This is needed in order to set the camera to look at.", gameObject);
        enabled = false;
    }

    private void Update()
    {
        // constantly look at the player
        transform.LookAt(m_target);
    }
}
