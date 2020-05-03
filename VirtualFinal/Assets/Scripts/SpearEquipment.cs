using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEquipment : Equipment
{
    [SerializeField] GameObject spearPrefab;
    private GameObject currSpear = null;
    private bool check = false;

    private void Update()
    {
        if (owner && currSpear == null && check)
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
        GameObject rightHand = i_player.GetComponent<PlayerController>().GetRightHand();

        if (currSpear == null)
        {
            currSpear = Instantiate(spearPrefab, new Vector3(rightHand.transform.position.x, i_player.transform.position.y, rightHand.transform.position.z) + i_player.transform.forward, i_player.transform.rotation * Quaternion.Euler(0.0f,90.0f,0.0f));
            currSpear.GetComponentInChildren<Spear>().forward = i_player.transform.forward;
            check = true;
            MusicManager.instance.PlayThrow();
        }
    }
}
