using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    internal GameObject Heldby = null;
    [SerializeField] GameObject parentObject;
    [SerializeField] float range;
    private float ogRange;    
    internal bool thrown = false;
    Vector3 pos = new Vector3();
    internal float yOG;
    Vector3 bounceOffset;
    internal bool overHole = false;

    private void Start()
    {
        ogRange = range;
    }

    private void OnDisable()
    {
        thrown = false;
        Heldby = null;
        range = ogRange;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !thrown)
        {
            other.gameObject.GetComponent<PlayerController>().SetCouldHold(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Wall") && thrown && !transform.parent.gameObject.CompareTag("Box"))
        {
            bounceOffset = (pos - transform.position).normalized;
            pos = transform.position - (bounceOffset);
            pos = new Vector3(pos.x, yOG, pos.z);
            MusicManager.instance.PlayThud();
        }
       else if (other.gameObject.CompareTag("Box") && thrown && transform.parent.gameObject.CompareTag("Bomb"))
        {
            bounceOffset = (pos - transform.position).normalized;
            pos = transform.position - (bounceOffset);
            pos = new Vector3(pos.x, yOG, pos.z);
            MusicManager.instance.PlayThud();
        }
        else if (other.gameObject.CompareTag("Wall") && thrown && transform.parent.gameObject.CompareTag("Box"))
        {
            transform.parent.gameObject.GetComponent<Box>().DestroyBox();
        }
        else if (other.gameObject.CompareTag("Shield") && thrown && transform.parent.gameObject.CompareTag("Box"))
        {
            if (!other.gameObject.Equals(Heldby.GetComponent<PlayerController>().m_shield))
            {
                transform.parent.gameObject.GetComponent<Box>().DestroyBox();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !thrown)
        {
            other.gameObject.GetComponent<PlayerController>().RemoveCould(this.gameObject);
        }
    }

    public void PickUp(GameObject i_player)
    {
        Heldby = i_player;
        yOG = parentObject.transform.position.y;
        transform.parent.gameObject.transform.position = i_player.transform.position + i_player.transform.up*2;
        parentObject.transform.SetParent(i_player.transform);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        MusicManager.instance.PlayLift();
    }

    public void Throw()
    {
        parentObject.transform.SetParent(null);

        if(Heldby.GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            range *= 1.25f;
        }

        pos = new Vector3(Heldby.transform.position.x, yOG, Heldby.transform.position.z) + Heldby.transform.forward*range;

        Heldby.GetComponent<PlayerController>().DisplayA(" ");
        thrown = true;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        MusicManager.instance.PlayThrow();
    }

    private void Update()
    {
        if (thrown)
        {
            parentObject.transform.position = Vector3.MoveTowards(parentObject.transform.position, pos, Time.deltaTime * 20);

            if (parentObject.transform.position == pos)
            {
                if (transform.parent.gameObject.CompareTag("Box"))
                {
                    thrown = false;
                    Heldby = null;
                    range = ogRange;
                        transform.parent.gameObject.GetComponent<Box>().DestroyBox();
                    if (overHole)
                    {
                        MusicManager.instance.PlayFalling();
                    }
                }
                else 
                {
                    thrown = false;
                    Heldby = null;
                    if (overHole)
                    {
                        MusicManager.instance.PlayFalling();
                        Destroy(this.transform.parent.gameObject);
                    }
                    else
                    {
                        MusicManager.instance.PlayThud();
                    }
                    range = ogRange;
                }
            }

        }
    }
}
