using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraTrigger : MonoBehaviour
{
    // Check for Trigger of player
    // Update main camera to appropriate angle

    // Check for trigger
    //debug.log trigger activated
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name} triggered {name}");
        }
    }
}
