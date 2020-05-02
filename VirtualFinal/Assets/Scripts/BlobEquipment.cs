using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobEquipment : Equipment
{
    [SerializeField] GameObject blobPrefab;
    private GameObject currBlob = null;
    private bool check = false;

    private void Update()
    {
        if(owner && currBlob == null && check)
        {
            switch(slot)
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

        if (currBlob == null)
        {
            currBlob = Instantiate(blobPrefab, new Vector3(rightHand.transform.position.x, i_player.transform.position.y, rightHand.transform.position.z) + (i_player.transform.forward*2.5f), i_player.transform.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f));
            currBlob.GetComponent<BlobShot>().MoveForward(i_player.transform.forward);
            check = true;
            MusicManager.instance.PlayShootSlime();
        }
    }
}
