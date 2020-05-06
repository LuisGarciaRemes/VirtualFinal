using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShot : MonoBehaviour
{
    [SerializeField] private float speed = 25;
    [SerializeField] private float lifeTime = 3;
    private float timer = 0.0f;
    [SerializeField] private GameObject splat;
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 3;
    private int tick = 1;

    // Update is called once per frame
    void Update()
    {
        if(timer>= lifeTime)
        {
            MusicManager.instance.PlaySplat();
            Instantiate(splat,transform.position,splat.transform.rotation);
            Destroy(this.gameObject);
        }
        else
        {
            timer += Time.deltaTime;
        }

        if(tick%100 == 0)
        {
            if(transform.localScale == new Vector3(1.5f,2f,2f))
            {
                transform.localScale = new Vector3(2f,2f,1.5f);
            }
            else
            {
                transform.localScale = new Vector3(1.5f, 2f, 2f);
            }
        }

        tick++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        MusicManager.instance.PlayBlobBounce();

        if(collision.gameObject.CompareTag("Shield"))
        {
            if (collision.transform.parent.gameObject.CompareTag("Enemy"))
            {
              
            }
            else if (collision.transform.parent.transform.parent.gameObject.CompareTag("Player"))
            {
                collision.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(-collision.impulse.normalized, knockback);
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().KnockBack(-collision.impulse.normalized,knockback);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().KnockBack(-collision.impulse.normalized, knockback);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Switch"))
        {
            collision.gameObject.GetComponent<Switch>().HitSwitch();
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            Vector3 force = collision.gameObject.transform.position - transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(-new Vector3(force.x,0.0f,force.z).normalized*speed, ForceMode.Impulse);
        }
    }

    public void MoveForward(Vector3 forward)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(forward*speed,ForceMode.Impulse);
    }
}
