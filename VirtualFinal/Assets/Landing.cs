using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : MonoBehaviour
{
    [SerializeField] private GameObject room;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().isFalling)
        {
            other.gameObject.GetComponent<PlayerController>().isFalling = false;
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, transform.parent.GetComponent<Hole>().landingLevel * 50 + 1.5f, other.gameObject.transform.position.z);
            transform.parent.GetComponent<Hole>().players.Remove(other.gameObject);
            CameraManager.instance.UpdateCameraPosition(other.gameObject.GetComponent<PlayerController>().m_playerID, room.transform.position);
            other.gameObject.GetComponent<PlayerController>().TakeDamage(10);
            MusicManager.instance.PlayThud();
            other.gameObject.GetComponent<PlayerController>().roomID = GameStateManager.instance.listOfRooms.IndexOf(room);
        }
    }
}
