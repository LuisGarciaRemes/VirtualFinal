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
            collision.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(-collision.impulse.normalized, knockback);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().KnockBack(-collision.impulse.normalized,knockback);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Switch"))
        {
            collision.gameObject.GetComponent<Switch>().HitSwitch();
        }
    }

    public void MoveForward(Vector3 forward)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(forward*speed,ForceMode.Impulse);
    }
}
