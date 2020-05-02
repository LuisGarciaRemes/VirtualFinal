using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShot : MonoBehaviour
{
    [SerializeField] private float speed = 25;
    private bool hitShield = false;
    [SerializeField] private float lifeTime = 3;
    private float timer = 0.0f;
    [SerializeField] private GameObject splat;
    [SerializeField] private int damage = 5;
    [SerializeField] private float knockback = 3;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        MusicManager.instance.PlayBlobBounce();

        if(collision.gameObject.CompareTag("Shield"))
        {
            collision.transform.parent.transform.parent.GetComponent<PlayerController>().KnockBack(-collision.impulse.normalized, knockback);
            hitShield = true;
        }
        else if (collision.gameObject.CompareTag("Player") && !hitShield)
        {
            collision.gameObject.GetComponent<PlayerController>().KnockBack(-collision.impulse.normalized,knockback);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

        hitShield = false;
    }

    public void MoveForward(Vector3 forward)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(forward*speed,ForceMode.Impulse);
    }
}
