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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().SetCouldHold(this.gameObject);
        }
        else if(other.gameObject.CompareTag("Wall") && thrown)
        {
            Debug.Log("Trigger wall");
            thrown = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().RemoveCould(this.gameObject);
        }
    }

    public void PickUp(GameObject i_player)
    {
        Heldby = i_player;
        transform.parent.gameObject.transform.position = i_player.transform.position + i_player.transform.up;
        parentObject.transform.SetParent(i_player.transform);
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    public void Throw()
    {
        parentObject.transform.SetParent(null);
        pos = Heldby.transform.position + Heldby.transform.forward*range;
        Heldby = null;
        thrown = true;
    }

    private void Update()
    {
        if(thrown)
        {
            parentObject.transform.position = Vector3.MoveTowards(parentObject.transform.position,pos,Time.deltaTime*10);

            if(parentObject.transform.position == pos)
            {
                thrown = false;
                gameObject.GetComponent<SphereCollider>().enabled = true;
            }

        }
    }
}
