using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private int healingAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().HealPlayer(healingAmount);
            MusicManager.instance.PlayHeartFill();
            Destroy(this.gameObject);
        }
    }
}
