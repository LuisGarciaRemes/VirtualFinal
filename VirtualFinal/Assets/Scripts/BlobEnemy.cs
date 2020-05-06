using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlobEnemy : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 3;
    [SerializeField] private float min = 0.5f;
    [SerializeField] private float max = 2.0f;
    NavMeshAgent navMeshAgent = null;
    private float wanderTimer = 0.0f;
    private float wanderDelay = 0.0f;
    [SerializeField] private GameObject splat;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        if(navMeshAgent == null)
        {
            Debug.LogError("BlobEnemy does not have navmeshagent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(wanderTimer >= wanderDelay)
        {
            wanderTimer = 0.0f;
            wanderDelay = UnityEngine.Random.Range(min,max);
            navMeshAgent.SetDestination(RandomNavSphere(transform.position,10.0f,-1));
            Instantiate(splat, transform.position, splat.transform.rotation);
        }
        else
        {
            wanderTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction;
        if (!other.gameObject.CompareTag("Wall"))
        {
            MusicManager.instance.PlayBlobBounce();
        }

        if (other.gameObject.CompareTag("Sword"))
        {
            direction = other.transform.parent.transform.parent.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            if (other.transform.parent.gameObject.CompareTag("Enemy"))
            {
                direction = other.transform.parent.position - transform.position;
                direction = new Vector3(direction.x, 0.0f, direction.z);
                other.transform.parent.GetComponent<Enemy>().KnockBack(direction.normalized, knockback);
            }
            else if (other.transform.parent.transform.parent.gameObject.CompareTag("Player"))
            {
                direction = other.transform.parent.transform.parent.position - transform.position;
                direction = new Vector3(direction.x, 0.0f, direction.z);
                other.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            direction = other.transform.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.gameObject.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else if(other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("Spikes"))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Box"))
        {
            other.gameObject.GetComponent<Box>().DestroyBox();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

}
