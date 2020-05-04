using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlobEnemy : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 3;

    [SerializeField] private Transform destination = null;
    NavMeshAgent navMeshAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        if(navMeshAgent == null)
        {
            Debug.LogError("BlobEnemy does not have navmeshagent");
        }
        else
        {
            SetDestination();
        }

    }

    private void SetDestination()
    {
        if(destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction;
        MusicManager.instance.PlayBlobBounce();

        if (other.gameObject.CompareTag("Sword"))
        {
            direction = other.transform.parent.transform.parent.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            direction = other.transform.parent.transform.parent.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            direction = other.transform.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.gameObject.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

}
