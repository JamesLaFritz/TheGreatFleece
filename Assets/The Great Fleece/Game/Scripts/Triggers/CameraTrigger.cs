using Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField, Tag] private string m_tag;

    [SerializeField] private Transform m_cameraProgressionAngle;


    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;
    private bool m_useVirtualCamera;

    private void Start()
    {
        m_useVirtualCamera = m_virtualCamera != null && m_virtualCamera.isActiveAndEnabled;

        if (m_cameraProgressionAngle != null) return;
        Debug.LogError("Camera Progression Angle has not been set please set one in the Inspector." +
                       " This is needed in order to set the main camera transform.", gameObject);
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ReSharper disable PossibleNullReferenceException
        if (!other.CompareTag(m_tag)) return;

        //Debug.Log($"{other.name} triggered {name}");
        if (m_useVirtualCamera)
        {
            Transform virtualCamera = m_virtualCamera.transform;
            virtualCamera.position = m_cameraProgressionAngle.position;
            virtualCamera.rotation = m_cameraProgressionAngle.rotation;
        }
        else
        {
            Transform mainCamera = Camera.main.transform;
            mainCamera.position = m_cameraProgressionAngle.position;
            mainCamera.rotation = m_cameraProgressionAngle.rotation;
        }
        // ReSharper restore PossibleNullReferenceException
    }
}
