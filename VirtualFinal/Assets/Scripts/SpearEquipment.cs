using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEquipment : Equipment
{
    [SerializeField] GameObject spearPrefab;
    private GameObject currSpear = null;

    public override void TriggerAbitily(GameObject i_player)
    {
        GameObject rightHand = i_player.GetComponent<PlayerController>().GetRightHand();

        if (currSpear == null)
        {
            currSpear = Instantiate(spearPrefab, new Vector3(rightHand.transform.position.x, spearPrefab.transform.position.y, rightHand.transform.position.z) + i_player.transform.forward, i_player.transform.rotation * Quaternion.Euler(0.0f,90.0f,0.0f));
            currSpear.GetComponentInChildren<Spear>().forward = i_player.transform.forward;
        }
    }
}
