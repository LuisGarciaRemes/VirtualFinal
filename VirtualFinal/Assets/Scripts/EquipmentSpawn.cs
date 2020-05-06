using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] EquipmentArray;
    private bool spawned = false;
    GameObject spawnedItem = null;
    private int itemIndex = 0;
    private bool needToCheck = true;
    internal float distanceToHeaven = 200;
    private float respawnDelay = 120;
    private float respawnTimer = 0.0f;

    internal int roomID = 0;
    [SerializeField] internal GameObject room;

    // Start is called before the first frame update
    void Start()
    {
        while(!spawned)
        {
            int index = Random.Range(0, EquipmentArray.Length);
            switch (index)
            {
                case 0:
                    if (!GameStateManager.instance.itemsSpawn[0])
                    {
                        spawned = true;
                        spawnedItem = Instantiate(EquipmentArray[0],transform.position,new Quaternion());
                        GameStateManager.instance.itemsSpawn[0] = true;
                        transform.position += transform.up * distanceToHeaven;
                        itemIndex = 0;
                    }
                    break;
                case 1:
                    if (!GameStateManager.instance.itemsSpawn[1])
                    {
                        spawned = true;
                        spawnedItem = Instantiate(EquipmentArray[1], transform.position, new Quaternion());
                        GameStateManager.instance.itemsSpawn[1] = true;
                        transform.position += transform.up * distanceToHeaven;
                        itemIndex = 1;
                    }
                    break;
                case 2:
                    if (!GameStateManager.instance.itemsSpawn[2])
                    {
                        spawned = true;
                        spawnedItem = Instantiate(EquipmentArray[2], transform.position, new Quaternion());
                        GameStateManager.instance.itemsSpawn[2] = true;
                        transform.position += transform.up * distanceToHeaven;
                        itemIndex = 2;
                    }
                    break;
                case 3:
                    if (!GameStateManager.instance.itemsSpawn[3])
                    {
                        spawned = true;
                        spawnedItem = Instantiate(EquipmentArray[3], transform.position, new Quaternion());
                        GameStateManager.instance.itemsSpawn[3] = true;
                        transform.position += transform.up * distanceToHeaven;
                        itemIndex = 3;
                    }
                    break;
            }
        }
    }

    private void Update()
    {
        if (needToCheck)
        {
            roomID = GameStateManager.instance.listOfRooms.IndexOf(room);
            needToCheck = false;
        }

        if(spawned)
        {
            if(spawnedItem.transform.position != transform.position - transform.up*distanceToHeaven)
            {
                if (respawnTimer >= respawnDelay)
                {
                    bool shouldRespawn = true;

                    foreach (PlayerController player in GameStateManager.instance.listOfPlayers)
                    {
                        if (player != null && player.roomID == roomID)
                        {
                            shouldRespawn = false;
                        }
                    }

                    if (shouldRespawn)
                    {
                        spawnedItem = Instantiate(EquipmentArray[itemIndex], transform.position - transform.up * distanceToHeaven, new Quaternion());
                        respawnTimer = 0.0f;
                    }
                }
                else
                {
                    respawnTimer += Time.deltaTime;
                }
            }
        }
    }

}
