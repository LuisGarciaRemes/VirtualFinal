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
        if (other.gameObject.CompareTag("Explosion"))
        {
            Destroy(this.gameObject);
        }
    }
}
