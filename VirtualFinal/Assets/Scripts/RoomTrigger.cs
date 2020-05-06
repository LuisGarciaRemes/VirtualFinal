using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private GameObject room;
    private int roomID = 0;

    private void Start()
    {
        if (!GameStateManager.instance.listOfRooms.Contains(room))
        {
            GameStateManager.instance.listOfRooms.Add(room);
        }

        roomID = GameStateManager.instance.listOfRooms.IndexOf(room);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CameraManager.instance.UpdateCameraPosition(other.gameObject.GetComponent<PlayerController>().m_playerID, room.transform.position);
            other.gameObject.GetComponent<PlayerController>().roomID = roomID;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.GetComponent<HelmetEnemy>())
            {
                other.gameObject.GetComponent<HelmetEnemy>().AbortHeadbutt();
            }
            
            other.gameObject.GetComponent<Enemy>().MoveAwayFromDoor();
        }
    }
}
