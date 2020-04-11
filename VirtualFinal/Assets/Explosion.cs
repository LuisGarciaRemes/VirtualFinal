using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int tickDelay;
    [SerializeField] private GameObject SmokeCloud;
    private int tickCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

        /*
        if(other.gameObject.GetComponent<>())
        {

        }
        */
    }

    private void Update()
    {
        if(tickCounter >= tickDelay)
        {
            Instantiate(SmokeCloud, transform.position + SmokeCloud.transform.position, SmokeCloud.transform.rotation);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            tickCounter++;
        }
    }
}
