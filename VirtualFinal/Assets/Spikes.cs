using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().SteppedOnSpike();
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Explosion"))
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().SteppedOnSpike();
            Destroy(this.gameObject);
        }
    }
}
