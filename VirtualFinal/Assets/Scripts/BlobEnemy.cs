using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlobEnemy : Enemy
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 3;
    [SerializeField] private GameObject splat;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction;

        if (other.gameObject.CompareTag("Sword"))
        {
            direction = other.transform.parent.transform.parent.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
            MusicManager.instance.PlayBlobBounce();
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            if (other.transform.parent.gameObject.CompareTag("Enemy"))
            {
                direction = other.transform.parent.position - transform.position;
                direction = new Vector3(direction.x, 0.0f, direction.z);
                other.transform.parent.GetComponent<Enemy>().KnockBack(direction.normalized, knockback);
                MusicManager.instance.PlayBlobBounce();
            }
            else if (other.transform.parent.transform.parent.gameObject.CompareTag("Player"))
            {
                direction = other.transform.parent.transform.parent.position - transform.position;
                direction = new Vector3(direction.x, 0.0f, direction.z);
                other.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
                MusicManager.instance.PlayBlobBounce();
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            direction = other.transform.position - transform.position;
            direction = new Vector3(direction.x, 0.0f, direction.z);
            other.gameObject.GetComponent<PlayerController>().KnockBack(direction.normalized, knockback);
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            MusicManager.instance.PlayBlobBounce();
        }
        else if(other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("Spikes"))
        {
            MusicManager.instance.PlayBlobBounce();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Box"))
        {
            MusicManager.instance.PlayBlobBounce();
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

}
