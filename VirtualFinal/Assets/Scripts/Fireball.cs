using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    [SerializeField] private float speed = 10;
    [SerializeField] private int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        MusicManager.instance.PlayFireballShoot();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {     
        if (other.gameObject.CompareTag("Wall"))
        {
            MusicManager.instance.PlayFireballHit();
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            MusicManager.instance.PlayFireballHit();
            Destroy(this.gameObject);
        }   
        else if (other.gameObject.CompareTag("Player"))
        {
            MusicManager.instance.PlayFireballHit();
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
