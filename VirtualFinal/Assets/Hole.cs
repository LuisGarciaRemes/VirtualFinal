using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    internal List<GameObject> players;

    [SerializeField] public int landingLevel;

    private void Start()
    {
        players = new List<GameObject>(); 
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !other.gameObject.GetComponent<PlayerController>().isDashing && !other.gameObject.GetComponent<PlayerController>().isFalling)
        {
            if (!players.Contains(other.gameObject))
            {
                players.Add(other.gameObject);
                other.gameObject.GetComponent<PlayerController>().m_velocity = Vector3.zero;
                other.gameObject.GetComponent<PlayerController>().isFalling = true;
                MusicManager.instance.PlayFalling();
            }
        }
    }

    private void Update()
    {
        foreach(GameObject player in players)
        {
            player.transform.position += new Vector3(0.0f,-5f * Time.deltaTime,0.0f);
        }
    }
}
