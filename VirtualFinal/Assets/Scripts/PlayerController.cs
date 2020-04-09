using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Gamestate
    internal int m_playerID;
    internal Vector3 m_spawnPoint;
    internal Camera m_camera;
    [SerializeField] private Material player2Mat;
    [SerializeField] private Material player3Mat;
    [SerializeField] private Material player4Mat;
    [SerializeField] private GameObject PlayerIndicatorUI;
    private GameObject playerHUD;


    //Movement
    private Vector3 m_velocity;
    private Rigidbody m_rb;
    [SerializeField] private float m_walkSpeed = 10;
    private bool isDashing = false;
    private bool canDash = true;
    [SerializeField] private float dashDist;
    [SerializeField] private float dashSpeed = 20;
    private Vector3 dashPos;
    private float dashTimer = 0.0f;
    [SerializeField] float dashDelay;

    //Sword
    [SerializeField] GameObject m_RightHand;
    private bool m_swingsword;
    private float m_swingRot;
    private float m_originalSwordRot;
    [SerializeField] private float m_finalSwordRot;
    [SerializeField] private float m_swordSpeed = 20.0f;

    //Shield
    [SerializeField] GameObject m_LeftHand;
    private bool m_holdshield;
    private float m_shieldRot;
    private float m_originalshieldRot;
    [SerializeField] private float m_finalShieldRot;
    [SerializeField] private float m_shieldSpeed = 20.0f;


    //Equipment
    private Equipment m_xEquipment = null;
    private Equipment m_yEquipment = null;
    internal Equipment m_tempEquipment = null;
    internal bool m_shouldCheckToEquip = false;

    // Start is called before the first frame update
    void Start()
    {
        m_playerID = GameStateManager.instance.AddPlayer(out playerHUD);
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
        m_holdshield = true;
        m_velocity = new Vector3(0.0f, 0.0f, 0.0f);

        switch (m_playerID)
        {
            case 2:
                gameObject.GetComponent<MeshRenderer>().material = player2Mat;
                break;
            case 3:
                gameObject.GetComponent<MeshRenderer>().material = player3Mat;
                break;
            case 4:
                gameObject.GetComponent<MeshRenderer>().material = player4Mat;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_rb.velocity = m_velocity * m_walkSpeed;

        if (isDashing)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashPos, Time.deltaTime * dashSpeed);

            if ((dashPos - transform.position).magnitude <= .1)
            {
                isDashing = false;
            }
        }

        if (!canDash)
        {
            if (dashTimer >= dashDelay)
            {
                canDash = true;
            }
            else
            {
                dashTimer += Time.deltaTime;
            }
        }

        if (m_swingsword)
        {
            SwingSword();
        }

        if (m_holdshield)
        {
            HoldShield();
        }

        if (m_velocity.magnitude != 0)
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
        if (!GameStateManager.instance.m_gameStarted && m_playerID != 0)
        {
            GameStateManager.instance.StartGame();
        }
    }

    private void OnSelectButton()
    {
        Debug.Log("Select");
    }

    private void OnAButton()
    {
        Debug.Log("A");

        if (m_tempEquipment != null)
        {
            m_shouldCheckToEquip = true;
            ShowIndicator("Press X Or\nY To Equip");
        }
    }

    private void OnBButton()
    {
        Debug.Log("B");

        if (canDash)
        {
            dashPos = transform.position + (transform.forward * dashDist);

            RaycastHit info;

            if (Physics.Raycast(transform.position, transform.forward, out info, dashDist, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                dashPos = info.point;
            }

            isDashing = true;
            canDash = false;
            dashTimer = 0.0f;
        }
    }

    private void OnYButton()
    {
        Debug.Log("Y");

        if (m_shouldCheckToEquip)
        {
            if (m_yEquipment)
            {
                if (Physics.Raycast(transform.position, transform.forward, 2, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    m_yEquipment.transform.position = transform.position - transform.forward * 2;
                }
                else
                {
                    m_yEquipment.transform.position = transform.position + transform.forward * 2;
                }
            }

            m_yEquipment = m_tempEquipment;
            playerHUD.transform.Find("YSprite").GetComponent<RawImage>().color = m_yEquipment.GetComponent<MeshRenderer>().material.color;
            m_shouldCheckToEquip = false;
            m_yEquipment.MoveToInventory();
            m_tempEquipment = null;
        }
        else if (m_yEquipment != null)
        {
            m_yEquipment.TriggerAbitily();
        }
    }

    private void OnXButton()
    {
        Debug.Log("X");

        if (m_shouldCheckToEquip)
        {
            if (m_xEquipment)
            {
                if (Physics.Raycast(transform.position, transform.forward, 2, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    m_xEquipment.transform.position = transform.position - transform.forward * 2;
                }
                else
                {
                    m_xEquipment.transform.position = transform.position + transform.forward * 2;
                }            
            }

            m_xEquipment = m_tempEquipment;
            playerHUD.transform.Find("XSprite").GetComponent<RawImage>().color = m_xEquipment.GetComponent<MeshRenderer>().material.color;
            m_xEquipment.MoveToInventory();
            m_shouldCheckToEquip = false;
            m_tempEquipment = null;
        }
        else if (m_xEquipment != null)
        {
            m_xEquipment.TriggerAbitily();
        }
    }

    private void OnRightTrigger()
    {
        Debug.Log("Right Trigger");
        if (!m_swingsword && (m_shieldRot != m_finalShieldRot))
        {
            m_swingsword = true;
            m_swingRot = m_finalSwordRot;
            m_RightHand.SetActive(true);
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
        m_RightHand.transform.localRotation = Quaternion.Lerp(m_RightHand.transform.localRotation, Quaternion.Euler(m_RightHand.transform.localRotation.eulerAngles.x, m_swingRot, m_RightHand.transform.localRotation.eulerAngles.z), Time.deltaTime * m_swordSpeed);

        Debug.Log(m_originalSwordRot);

        if (((m_RightHand.transform.localRotation.eulerAngles.y <= m_originalSwordRot + 1 && m_RightHand.transform.localRotation.eulerAngles.y >= m_originalSwordRot - 1) && m_swingRot == m_originalSwordRot))
        {
            Debug.Log("Reached");
            m_swingsword = false;
            m_RightHand.SetActive(false);
        }

        if (((m_RightHand.transform.localRotation.eulerAngles.y <= m_finalSwordRot && m_RightHand.transform.localRotation.eulerAngles.y >= m_finalSwordRot - 1) && m_swingRot == m_finalSwordRot))
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

    public void ShowIndicator(string i_text)
    {
        PlayerIndicatorUI.SetActive(true);
        PlayerIndicatorUI.GetComponentInChildren<TextMesh>().text = i_text;
    }

    public void HideIndicator()
    {
        PlayerIndicatorUI.SetActive(false);
    }
}
