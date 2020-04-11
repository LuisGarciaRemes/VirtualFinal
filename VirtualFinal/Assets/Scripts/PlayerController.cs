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
    private int health = 100;
    private float immuneTimer = 0.0f;
    [SerializeField] private float immuneDelay = 0.0f;
    private int immuneTick = 0;
    private bool isImmune = false;


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

    //Interacting
    private GameObject couldHold = null;
    private GameObject holding = null;

    // Start is called before the first frame update
    void Start()
    {
        m_playerID = GameStateManager.instance.AddPlayer(out playerHUD);
        transform.position = GameStateManager.instance.GetInitialSpawnPoint(m_playerID);
        m_camera = GameObject.Find("Player" + m_playerID + "Camera").GetComponent<Camera>();
        gameObject.GetComponent<PlayerInput>().camera = m_camera;
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

        if(isImmune)
        {
            FlashWhileImmune();
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
        if (!GameStateManager.instance.m_gameStarted && m_playerID != 0)
        {
            GameStateManager.instance.StartGame();
        }
    }

    private void OnSelectButton()
    {

    }

    private void OnAButton()
    {
        if (m_tempEquipment != null)
        {
            m_shouldCheckToEquip = true;
            ShowIndicator("Press X Or\nY To Equip");
        }
        else if(couldHold != null && holding == null && !m_holdshield)
        {
            couldHold.GetComponent<PickUpObject>().PickUp(this.gameObject);
            HideIndicator();
            holding = couldHold;
            couldHold = null;
        }
        else if(holding)
        {
            holding.GetComponent<PickUpObject>().Throw();
            holding = null;
        }
        
    }

    private void OnBButton()
    {
        if (canDash)
        {
            dashPos = transform.position + (transform.forward * dashDist);

            RaycastHit info;

            if (Physics.Raycast(transform.position, transform.forward, out info, dashDist, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                dashPos = info.point - (transform.forward/2);
            }

            isDashing = true;
            canDash = false;
            dashTimer = 0.0f;
        }
    }

    private void OnYButton()
    {
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
        else if (m_yEquipment != null && !m_holdshield && !holding && !m_swingsword)
        {
            m_yEquipment.TriggerAbitily(this.gameObject);
        }
    }

    private void OnXButton()
    {
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
        else if (m_xEquipment != null && !m_holdshield && !holding && !m_swingsword)
        {
            m_xEquipment.TriggerAbitily(this.gameObject);
        }
    }

    private void OnRightTrigger()
    {
        if (!m_swingsword && (m_shieldRot != m_finalShieldRot) && !holding)
        {
            m_swingsword = true;
            m_swingRot = m_finalSwordRot;
            m_RightHand.SetActive(true);
            MusicManager.instance.PlaySwing();
        }
    }

    private void OnLeftTrigger()
    {
        if (!m_holdshield && !holding)
        {
            m_holdshield = true;
            m_shieldRot = m_finalShieldRot;
        }
    }

    private void OnLeftTriggerRelease()
    {
        if (m_holdshield)
        {
            m_shieldRot = m_originalshieldRot;
        }
    }

    private void SwingSword()
    {
        m_RightHand.transform.localRotation = Quaternion.Lerp(m_RightHand.transform.localRotation, Quaternion.Euler(m_RightHand.transform.localRotation.eulerAngles.x, m_swingRot, m_RightHand.transform.localRotation.eulerAngles.z), Time.deltaTime * m_swordSpeed);

        /*
        if (((m_RightHand.transform.localRotation.eulerAngles.y <= m_originalSwordRot + 1 && m_RightHand.transform.localRotation.eulerAngles.y >= m_originalSwordRot - 1) && m_swingRot == m_originalSwordRot))
        {
            m_swingsword = false;
            m_RightHand.transform.Find("Sword").GetComponent<Sword>().SetBlocked(false);
            m_RightHand.transform.localRotation = Quaternion.Euler(m_RightHand.transform.localRotation.eulerAngles.x, m_originalSwordRot, m_RightHand.transform.localRotation.eulerAngles.z);
        }
        */


        if (((m_RightHand.transform.localRotation.eulerAngles.y <= m_finalSwordRot && m_RightHand.transform.localRotation.eulerAngles.y >= m_finalSwordRot - 1) && m_swingRot == m_finalSwordRot))
        {
            m_RightHand.SetActive(false);
            m_swingRot = m_originalSwordRot;

            m_swingsword = false;
            m_RightHand.transform.Find("Sword").GetComponent<Sword>().SetBlocked(false);
            m_RightHand.transform.localRotation = Quaternion.Euler(m_RightHand.transform.localRotation.eulerAngles.x, m_originalSwordRot, m_RightHand.transform.localRotation.eulerAngles.z);
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

    public void TakeDamage(int i_damage)
    {
        if (!isImmune)
        {
            health -= i_damage;
            Debug.Log("Ouch!");
            Debug.Log("Player " + m_playerID + " Health is " + health);
            BecomeImmune();
            UpdateHealthIndicator();
            MusicManager.instance.PlayDamagedPlayer();
        }
    }

    public void BecomeImmune()
    {
        isImmune = true;
        immuneTimer = 0.0f;
    }

    private void UpdateHealthIndicator()
    {
        playerHUD.transform.Find("HealthSprite").GetComponentInChildren<Text>().text = health.ToString();
    }

    public void SetCouldHold(GameObject i_pickup)
    {
        ShowIndicator("A To Pick Up");
        couldHold = i_pickup;
    }

    public void RemoveCould(GameObject i_pickup)
    {
        if (couldHold != null && couldHold.Equals(i_pickup))
        {
            HideIndicator();
            couldHold = null;
        }
    }

    public GameObject GetRightHand()
    {
        return m_RightHand;
    }

    private void FlashWhileImmune()
    {
        if(immuneTimer >= immuneDelay)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            isImmune = false;
            immuneTick = 0;
        }
        else
        {
            immuneTimer += Time.deltaTime;
            immuneTick++;

            if(immuneTick % 15 == 0)
            {
                if(gameObject.GetComponent<MeshRenderer>().enabled)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }
}
