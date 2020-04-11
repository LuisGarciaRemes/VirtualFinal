using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEquipment : Equipment
{
    [SerializeField] GameObject bombPrefab;
    private GameObject currBomb = null;

    public override void TriggerAbitily(GameObject i_player)
    {
        if(currBomb == null)
        {
           currBomb = Instantiate(bombPrefab, new Vector3(i_player.transform.position.x, bombPrefab.transform.position.y, i_player.transform.position.z) + i_player.transform.forward, new Quaternion());
        }
    }

}
