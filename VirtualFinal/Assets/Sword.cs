using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private GameObject player;
    [SerializeField] GameObject shield;
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
            other.gameObject.GetComponent<PlayerController>().TakeDamage(10);
        }
        else if (other.gameObject.CompareTag("Shield") && !other.gameObject.Equals(shield))
        {
            MusicManager.instance.PlayStrike();
            SetBlocked(true);
            transform.parent.gameObject.SetActive(false);
        }

    }

    public void SetBlocked(bool i_bool)
    {
        isBlocked = i_bool;
    }
}
