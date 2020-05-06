using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbutt : MonoBehaviour
{
    internal bool isHeadbutting = false;
    [SerializeField] private int damage = 10;
    internal bool wasBlocked = false;
    [SerializeField] private GameObject shield;
    [SerializeField] private HelmetEnemy parent;

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.Equals(shield) && isHeadbutting)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                MusicManager.instance.PlayStrike();
                wasBlocked = true;
                isHeadbutting = false;
                parent.stopRotating = true;
            }
            else if (other.gameObject.CompareTag("Player") && !wasBlocked)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                parent.stopRotating = true;
                isHeadbutting = false;
            }
            else if (other.gameObject.CompareTag("Box"))
            {
                other.gameObject.GetComponent<Box>().DestroyBox();
                parent.stopRotating = true;
                isHeadbutting = false;
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                MusicManager.instance.PlayStrike();
                parent.GetComponent<HelmetEnemy>().AbortHeadbutt();
            }
            wasBlocked = false;
        }
    }

}
