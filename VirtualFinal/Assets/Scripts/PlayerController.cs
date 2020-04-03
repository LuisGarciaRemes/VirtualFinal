using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private int m_playerID;
    internal Vector3 m_spawnPoint;
    private Vector3 m_velocity;
    private Rigidbody m_rb;
    private float m_speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        m_playerID = GameStateManager.instance.AddPlayer();
        transform.position = GameStateManager.instance.GetInitialSpawnPoint(m_playerID);
        gameObject.GetComponent<PlayerInput>().camera = GameObject.Find("Player"+m_playerID+"Camera").GetComponent<Camera>();
        Debug.Log("Created Player " + m_playerID);
        m_rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = m_velocity*m_speed;
    }

    private void OnMove(InputValue value)
    {
       m_velocity.x = value.Get<Vector2>().x;
        m_velocity.z = value.Get<Vector2>().y;
        m_velocity.y = 0.0f;
    }

    private void OnAButton()
    {
        GameStateManager.instance.SetSplitScreen(true);
    }
}
