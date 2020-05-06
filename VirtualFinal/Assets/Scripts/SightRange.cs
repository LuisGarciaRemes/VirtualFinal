using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightRange : MonoBehaviour
{
    internal List<GameObject> players = new List<GameObject>();
    internal int roomID = 0;
    internal bool isSet = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !players.Contains(other.gameObject) && other.gameObject.GetComponent<PlayerController>().roomID == roomID)
        {           
            if (players.Count == 0)
            {
                MusicManager.instance.PlaySpottedByEnemy();
            }

            players.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && players.Contains(other.gameObject))
        {
            players.Remove(other.gameObject);
        }
    }

    public GameObject FindNearestPlayer()
    {
        float nearestDist = 500.0f;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            if (player != null)
            {
                Vector3 temp = player.transform.position - this.transform.position;

                if (temp.magnitude < nearestDist)
                {
                    nearestDist = temp.magnitude;
                    closestPlayer = player;
                }
            }
        }

        return closestPlayer;
    }

    public void SetRoomId(int id)
    {
        roomID = id;
        isSet = true;
    }
}
