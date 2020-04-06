using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private Vector3 m_position;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = m_position;
            other.gameObject.GetComponent<PlayerController>().m_tempEquipment = this;
        }
    }

    public virtual void TriggerAbitily()
    {
        Debug.Log("Ability Used");
    }

}
