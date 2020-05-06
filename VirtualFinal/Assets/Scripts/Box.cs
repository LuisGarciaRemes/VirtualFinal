using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    private bool needToCheck = true;
    internal float distanceToHeaven = 200;
    private float respawnDelay = 120;
    private float respawnTimer = 0.0f;
    internal bool destroyed = false;
    [SerializeField] GameObject breakSprite;
    Vector3 originalPos;

    internal int roomID = 0;
    [SerializeField] internal GameObject room;
    [SerializeField] internal PickUpObject pickUpObject;

    private void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (needToCheck)
        {
            roomID = GameStateManager.instance.listOfRooms.IndexOf(room);
            needToCheck = false;
        }

        if (destroyed)
        {
            if (respawnTimer >= respawnDelay)
            {
                bool shouldRespawn = true;

                foreach (PlayerController player in GameStateManager.instance.listOfPlayers)
                {
                    if (player != null && player.roomID == roomID)
                    {
                        shouldRespawn = false;
                    }
                }

                if (shouldRespawn)
                {
                    transform.position = originalPos;
                    destroyed = false;
                    respawnTimer = 0.0f;
                    transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
                    pickUpObject.enabled = true;
                }
            }
            else
            {
                respawnTimer += Time.deltaTime;
            }
        }

        if(pickUpObject.Heldby != null && gameObject.layer == 0 && !pickUpObject.thrown)
        {
            gameObject.layer = 9;
        }
        else if (pickUpObject.thrown && gameObject.layer == 9)
        {
            gameObject.layer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pickUpObject.thrown && !destroyed)
        {
            if (collision.gameObject.CompareTag("Player") && !collision.gameObject.Equals(pickUpObject.Heldby))
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(10);
                DestroyBox();
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
                DestroyBox();
            }
            else if (collision.gameObject.CompareTag("Box"))
            {
                collision.gameObject.GetComponent<Box>().DestroyBox();
                DestroyBox();
            }

        }
    }

    public void DestroyBox()
    {
        Instantiate(breakSprite,transform.position+transform.up,Quaternion.Euler(90.0f, transform.rotation.eulerAngles.y,0.0f));
        destroyed = true;
        GameStateManager.instance.SpawnHeart(transform.position);
        transform.position += transform.up * distanceToHeaven;
        pickUpObject.enabled = false;
        MusicManager.instance.PlayBoxBreak();
    }
}
