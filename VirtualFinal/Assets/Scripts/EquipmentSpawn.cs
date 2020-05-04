using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] EquipmentArray;
    private bool spawned = false;

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
                        Instantiate(EquipmentArray[0],transform.position,new Quaternion());
                        GameStateManager.instance.itemsSpawn[0] = true;
                        Destroy(this.gameObject);
                    }
                    break;
                case 1:
                    if (!GameStateManager.instance.itemsSpawn[1])
                    {
                        spawned = true;
                        Instantiate(EquipmentArray[1], transform.position, new Quaternion());
                        GameStateManager.instance.itemsSpawn[1] = true;
                        Destroy(this.gameObject);
                    }
                    break;
                case 2:
                    if (!GameStateManager.instance.itemsSpawn[2])
                    {
                        spawned = true;
                        Instantiate(EquipmentArray[2], transform.position, new Quaternion());
                        GameStateManager.instance.itemsSpawn[2] = true;
                        Destroy(this.gameObject);
                    }
                    break;
                case 3:
                    if (!GameStateManager.instance.itemsSpawn[3])
                    {
                        spawned = true;
                        Instantiate(EquipmentArray[3], transform.position, new Quaternion());
                        GameStateManager.instance.itemsSpawn[3] = true;
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }

}
