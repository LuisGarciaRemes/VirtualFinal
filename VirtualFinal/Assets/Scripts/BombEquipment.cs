using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEquipment : Equipment
{
    [SerializeField] GameObject bombPrefab;
    private GameObject currBomb = null;
    private bool check = false;

    private void Update()
    {
        if (owner && currBomb == null && check)
        {
            switch (slot)
            {
                case 'x':
                    owner.DisplayX();
                    break;
                case 'y':
                    owner.DisplayY();
                    break;
                default:
                    break;
            }

            check = false;
        }
    }

    public override void TriggerAbitily(GameObject i_player)
    {
        if(currBomb == null)
        {
           currBomb = Instantiate(bombPrefab, new Vector3(i_player.transform.position.x, i_player.transform.position.y, i_player.transform.position.z) + i_player.transform.forward, new Quaternion());
            check = true;
            MusicManager.instance.PlayThud();
        }
    }

}
