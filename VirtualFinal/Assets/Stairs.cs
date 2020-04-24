using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject room;
    internal bool canUse = true;
    internal GameObject player = null;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && canUse)
        {
            other.gameObject.transform.position = exit.transform.position + new Vector3(0.0f,0.6f,0.0f);
            CameraManager.instance.UpdateCameraPosition(other.gameObject.GetComponent<PlayerController>().m_playerID, room.transform.position);
            exit.GetComponent<Stairs>().canUse = false;
            canUse = false;
            exit.GetComponent<Stairs>().player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject && other.gameObject.Equals(player))
        {
            canUse = true;
            exit.GetComponent<Stairs>().canUse = true;
            player = null;
        }
    }
}
