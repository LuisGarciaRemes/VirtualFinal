using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{

    private Vector3 originalPos;
    private Vector3 finalPos;
    [SerializeField] private float speed = 5;
    private Vector3 currDestination;
    private bool activated = false;
    [SerializeField] Blade pair;

    private void Start()
    {
        originalPos = transform.position;
        finalPos = new Vector3((originalPos.x+pair.transform.position.x) / 2,originalPos.y,(originalPos.z + pair.transform.position.z)/2);
    }

    private void Update()
    {
        if(activated)
        {
            transform.position = Vector3.MoveTowards(transform.position,currDestination,Time.deltaTime * speed);

            if(transform.position == currDestination)
            {
               if(currDestination == finalPos)
                {
                    Retract();
                }
                else
                {
                    activated = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(10);
        }
    }

    private void Retract()
    {
        if (activated)
        {
            currDestination = originalPos;
        }
    }

    public void Extend()
    {
        if (!activated)
        {
            currDestination = finalPos;
            activated = true;
        }
    }
}
