using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private GameObject spear;
    [SerializeField] private int damage;
    private bool shouldStop = false;
    private float timer = 0.0f;
    [SerializeField] private float disappearDelay;
    internal Vector3 forward;
    private bool hitShield = false;

    // Update is called once per frame
    void Update()
    {
        if (!shouldStop)
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
            hitShield = true;
            MusicManager.instance.PlaySpearStick();
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            shouldStop = true;
            hitShield = true;
            disappearDelay = 0.25f;
            MusicManager.instance.PlayStrike();
        }
        else if (other.gameObject.CompareTag("Player") && !hitShield)
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(spear);
        }
    }
}
