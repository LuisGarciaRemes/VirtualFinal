using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 m_position;

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CameraManager.instance.UpdateCameraPosition(other.gameObject.GetComponent<PlayerController>().m_playerID,m_position);
        }
    }
}
