using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesEquipment : Equipment
{
    private float timer = 0.0f;
   [SerializeField] private float delay;
    private bool canThrow = true;
    [SerializeField] GameObject spikes;

    private void Update()
    {
        if (!canThrow)
        {
            if (timer >= delay)
            {
                timer = 0.0f;
                canThrow = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public override void TriggerAbitily(GameObject i_player)
    {
        if (canThrow)
        {
            Vector3 leftVector = new Vector3();
            Vector3 middleVector = new Vector3();
            Vector3 rightVector = new Vector3();

            if (i_player.GetComponent<PlayerController>().m_rb.velocity == new Vector3())
            {
                leftVector = i_player.transform.position + (i_player.transform.forward) * 3 - (i_player.transform.right) * 2f;
                middleVector = i_player.transform.position + (i_player.transform.forward) * 3;
                rightVector = i_player.transform.position + (i_player.transform.forward) * 3 + (i_player.transform.right) * 2f;
            }
            else
            {
                leftVector = i_player.transform.position - (i_player.transform.forward) * 2 - (i_player.transform.right) * 2f;
                middleVector = i_player.transform.position - (i_player.transform.forward) * 2;
                rightVector = i_player.transform.position - (i_player.transform.forward) * 2 + (i_player.transform.right) * 2f;
            }

            Instantiate(spikes,new Vector3(leftVector.x,.1f,leftVector.z),spikes.transform.rotation);
            Instantiate(spikes, new Vector3(middleVector.x, .1f, middleVector.z), spikes.transform.rotation);
            Instantiate(spikes, new Vector3(rightVector.x, .1f, rightVector.z), spikes.transform.rotation);

            canThrow = false;
        }
    }
}
