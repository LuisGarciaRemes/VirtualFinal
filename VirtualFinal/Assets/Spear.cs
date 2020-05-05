using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private GameObject spear;
    [SerializeField] private int damage;
    internal bool shouldStop = false;
    private float timer = 0.0f;
    [SerializeField] private float disappearDelay;
    internal Vector3 forward = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (!shouldStop && forward != Vector3.zero)
        {
            spear.transform.position = Vector3.MoveTowards(spear.transform.position, spear.transform.position + forward, Time.deltaTime * speed);
        }

        if(shouldStop)
        {
            if(timer >= disappearDelay)
            {
                Destroy(spear);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.CompareTag("Wall"))
        {
            shouldStop = true;
            MusicManager.instance.PlaySpearStick();
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            shouldStop = true;
            MusicManager.instance.PlayStrike();
        }
        else if (other.gameObject.CompareTag("Player") && !shouldStop)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(spear);
        }
        else if (other.gameObject.CompareTag("Enemy") && !shouldStop)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(spear);
        }
        else if (other.gameObject.CompareTag("Switch"))
        {
            other.gameObject.GetComponent<Switch>().HitSwitch();
            shouldStop = true;
            MusicManager.instance.PlayStrike();
        }
        else if (other.gameObject.CompareTag("Blob"))
        {
            shouldStop = true;
        }
    }
}
