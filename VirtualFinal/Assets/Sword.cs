using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private GameObject player;
    private bool isBlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !other.gameObject.Equals(player) && !isBlocked)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(5);
        }

        if (other.gameObject.CompareTag("Shield") && !other.gameObject.Equals(player.transform.Find("LeftHand").Find("Shield")))
        {
            MusicManager.instance.PlayStrike();
            SetBlocked(true);
            Debug.Log("Blocked");
        }
    }

    public void SetBlocked(bool i_bool)
    {
        isBlocked = i_bool;
    }
}
