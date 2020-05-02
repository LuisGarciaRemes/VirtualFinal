using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private GameObject Heldby = null;
    [SerializeField] GameObject parentObject;
    [SerializeField] float range;
    bool thrown = false;
    Vector3 pos = new Vector3();
    private float yOG;
    Vector3 bounceOffset;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !thrown)
        {
            other.gameObject.GetComponent<PlayerController>().SetCouldHold(this.gameObject);
        }
        else if(other.gameObject.CompareTag("Wall") && thrown)
        {
            bounceOffset = (pos - transform.position).normalized;
            pos = transform.position - (bounceOffset);
            pos = new Vector3(pos.x,yOG,pos.z);
            MusicManager.instance.PlayThud();
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
        Heldby = null;
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
                thrown = false;
                MusicManager.instance.PlayThud();
            }

        }
    }
}
