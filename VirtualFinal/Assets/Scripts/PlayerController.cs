using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    internal int m_playerID;
    internal Vector3 m_spawnPoint;
    private Vector3 m_velocity;
    private Rigidbody m_rb;
    [SerializeField] private float m_walkSpeed = 10;
    [SerializeField] GameObject m_RightHand;
    [SerializeField] GameObject m_LeftHand;
    private bool m_swingsword;
    private float m_swingRot;
    private float m_originalSwordRot;
    [SerializeField] private float m_finalSwordRot;
    private bool m_holdshield;
    private float m_shieldRot;
    private float m_originalshieldRot;
    [SerializeField] private float m_finalShieldRot;

    [SerializeField] private float m_swordSpeed = 20.0f;
    [SerializeField] private float m_shieldSpeed = 20.0f;

    internal Camera m_camera;

    // Start is called before the first frame update
    void Start()
    {
        m_playerID = GameStateManager.instance.AddPlayer();
        transform.position = GameStateManager.instance.GetInitialSpawnPoint(m_playerID);
        m_camera = GameObject.Find("Player" + m_playerID + "Camera").GetComponent<Camera>();
        gameObject.GetComponent<PlayerInput>().camera = m_camera;
        Debug.Log("Created Player " + m_playerID);
        m_rb = gameObject.GetComponent<Rigidbody>();
        m_swingsword = false;
        m_originalSwordRot = m_RightHand.transform.rotation.eulerAngles.y;
        m_swingRot = m_originalSwordRot;
        m_originalshieldRot = m_LeftHand.transform.rotation.eulerAngles.y;
        m_shieldRot = m_originalshieldRot;
        m_holdshield = false;
        m_velocity = new Vector3(0.0f,0.0f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = m_velocity*m_walkSpeed;

        if(m_swingsword)
        {
            SwingSword();
        }

        if (m_holdshield)
        {
            HoldShield();
        }

        if(m_velocity.magnitude != 0)
        {
            transform.rotation = Quaternion.Euler(0.0f, -90.0f + Mathf.Atan2(m_velocity.z, -m_velocity.x) * 57.2958f, 0.0f);
        }
    }

    private void OnMove(InputValue value)
    {
       m_velocity.x = value.Get<Vector2>().x;
        m_velocity.z = value.Get<Vector2>().y;
        m_velocity.y = 0.0f;
    }

    private void OnStartButton()
    {
        Debug.Log("Start");
        GameStateManager.instance.SetSplitScreen(true);
    }

    private void OnSelectButton()
    {
        Debug.Log("Select");
    }

    private void OnAButton()
    {
        Debug.Log("A");
    }

    private void OnBButton()
    {
        Debug.Log("B");
    }

    private void OnYButton()
    {
        Debug.Log("Y");
    }

    private void OnXButton()
    {
        Debug.Log("X");
    }

    private void OnRightTrigger()
    {
        Debug.Log("Right Trigger");
        if (!m_swingsword && (m_shieldRot != m_finalShieldRot))
        {
            m_swingsword = true;
            m_swingRot = m_finalSwordRot;
        }
    }

    private void OnLeftTrigger()
    {
        Debug.Log("Left Trigger Pressed");
        if (!m_holdshield)
        {
            m_holdshield = true;
            m_shieldRot = m_finalShieldRot;
        }
    }

    private void OnLeftTriggerRelease()
    {
        Debug.Log("Left Trigger Release");
        if (m_holdshield)
        {
            m_shieldRot = m_originalshieldRot;
        }
    }

    private void SwingSword()
    {
        m_RightHand.transform.localRotation = Quaternion.Lerp(m_RightHand.transform.localRotation, Quaternion.Euler(m_RightHand.transform.localRotation.eulerAngles.x,m_swingRot, m_RightHand.transform.localRotation.eulerAngles.z),Time.deltaTime * m_swordSpeed);

        if (((m_RightHand.transform.localRotation.eulerAngles.y <= m_originalSwordRot && m_RightHand.transform.localRotation.eulerAngles.y >= m_originalSwordRot - 1) && m_swingRot == m_originalSwordRot))
        {
            m_swingsword = false;
        }

        if(((m_RightHand.transform.localRotation.eulerAngles.y <= m_finalSwordRot + 361 && m_RightHand.transform.localRotation.eulerAngles.y >= m_finalSwordRot + 360) && m_swingRot == m_finalSwordRot))
        {
            m_swingRot = m_originalSwordRot;
        }
    }

    private void HoldShield()
    {
        m_LeftHand.transform.localRotation = Quaternion.Lerp(m_LeftHand.transform.localRotation, Quaternion.Euler(m_LeftHand.transform.localRotation.eulerAngles.x, m_shieldRot, m_LeftHand.transform.localRotation.eulerAngles.z), Time.deltaTime * m_shieldSpeed);

        if (((m_LeftHand.transform.localRotation.eulerAngles.y <= m_originalshieldRot + 1 && m_LeftHand.transform.localRotation.eulerAngles.y >= m_originalshieldRot) && m_shieldRot == m_originalshieldRot))
        {
            m_holdshield = false;
        }
    }
}
