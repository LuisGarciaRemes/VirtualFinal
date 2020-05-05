using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    private bool needToCheck = true;
    internal float distanceToHeaven = 200;
    private float respawnDelay = 120;
    private float respawnTimer = 0.0f;
    internal bool destroyed = false;

    internal int roomID = 0;
    [SerializeField] internal GameObject room;
    [SerializeField] internal PickUpObject pickUpObject;

    // Update is called once per frame
    void Update()
    {
        if (needToCheck)
        {
            roomID = GameStateManager.instance.listOfRooms.IndexOf(room);
            needToCheck = false;
        }

        if (destroyed)
        {
            if (respawnTimer >= respawnDelay)
            {
                bool shouldRespawn = true;

                foreach (PlayerController player in GameStateManager.instance.listOfPlayers)
                {
                    if (player.roomID == roomID)
                    {
                        shouldRespawn = false;
                    }
                }

                if (shouldRespawn)
                {
                    transform.position -= transform.up * distanceToHeaven;
                    destroyed = false;
                    respawnTimer = 0.0f;
                    transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
                }
            }
            else
            {
                respawnTimer += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pickUpObject.thrown && !destroyed)
        {
            if (collision.gameObject.CompareTag("Player") && !collision.gameObject.Equals(pickUpObject.Heldby))
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(10);
                DestroyBox();
            }
        }
    }

    public void DestroyBox()
    {
        destroyed = true;
        GameStateManager.instance.SpawnHeart(transform.position);
        transform.position += transform.up * distanceToHeaven;
    }
}
