using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private Vector3 m_position;
    internal char slot = ' ';
    internal PlayerController owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().m_tempEquipment = this;
            other.gameObject.GetComponent<PlayerController>().ShowIndicator("Press A\nTo Interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().m_tempEquipment = null;
            other.gameObject.GetComponent<PlayerController>().m_shouldCheckToEquip = false;
            other.gameObject.GetComponent<PlayerController>().HideIndicator();
        }
    }

    public void MoveToInventory()
    {
        transform.position = m_position;
    }

    public virtual void TriggerAbitily(GameObject i_player)
    {
        Debug.Log("Ability Used");
    }

    public void SetOwnerAndSlot(PlayerController i_owner, char i_slot)
    {
        owner = i_owner;
        slot = i_slot;
    }

}
