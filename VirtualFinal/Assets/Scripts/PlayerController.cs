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
    private GameObject playerHUD;
    private int health = 100;
    private float immuneTimer = 0.0f;
    [SerializeField] private float immuneDelay = 0.0f;
    private int immuneTick = 0;
    private bool isImmune = false;
    internal int roomID = 0;


    //Movement
    internal Vector3 m_velocity;
    internal Rigidbody m_rb;
    [SerializeField] private float m_walkSpeed = 10;
    internal bool isDashing = false;
    internal bool canDash = true;
    [SerializeField] private float dashDist;
    [SerializeField] private float dashSpeed = 20;
    private Vector3 dashPos;
    private float dashTimer = 0.0f;
    [SerializeField] float dashDelay;
    internal bool isFalling = false;

    //Sword
    [SerializeField] GameObject m_RightHand;
    private bool m_swingsword;
    private float m_swingRot;
    private float m_originalSwordRot;
    [SerializeField] private float m_finalSwordRot;
    [SerializeField] private float m_swordSpeed = 20.0f;
    private GameObject m_sword;
    private Quaternion lastSwordRot = new Quaternion();

    //Shield
    [SerializeField] GameObject m_LeftHand;
    internal GameObject m_shield;
    internal bool m_holdshield;
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

    //Spikes
    private float slowTimer = 0.0f;
    [SerializeField] private float slowDelay;
    private bool isSlowed = false;

    //HUD
    private Text yText;
    private Text bText;
    private Text xText;
    private Text aText;
    private Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        m_playerID = GameStateManager.instance.AddPlayer(out playerHUD);
        GameStateManager.instance.listOfPlayers.Add(this);
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

        Transform Button = playerHUD.transform.Find("Buttons");


        bText = Button.Find("BSprite").GetComponentInChildren<Text>();
        yText = Button.Find("YSprite").GetComponentInChildren<Text>();
        xText = Button.Find("XSprite").GetComponentInChildren<Text>();
        aText = Button.Find("ASprite").GetComponentInChildren<Text>();
        healthText = playerHUD.transform.Find("HealthSprite").GetComponentInChildren<Text>();
        m_sword = m_RightHand.transform.Find("Sword").gameObject;
        m_shield = m_LeftHand.transform.Find("Shield").gameObject;
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
                DisplayB();
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

        if(isSlowed)
        {
            CheckSlowed();
        }

        CheckHealth();

        if(isFalling && isImmune)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            isImmune = false;
            immuneTick = 0;
        }

    }

    private void OnMove(InputValue value)
    {
        if (!isFalling)
        {
            m_velocity.x = value.Get<Vector2>().x;
            m_velocity.z = value.Get<Vector2>().y;
            m_velocity.y = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(5);
        }
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
        Application.Quit();
    }

    private void OnAButton()
    {
        if(holding)
        {
            holding.GetComponent<PickUpObject>().Throw();
            holding = null;
        }
        else if(couldHold != null && holding == null && !m_holdshield)
        {
            couldHold.GetComponent<PickUpObject>().PickUp(this.gameObject);
            aText.text = "Throw";
            holding = couldHold;
            couldHold = null;
        }
        else if(m_tempEquipment != null)
        {
            m_shouldCheckToEquip = true;
            xText.text = "Equip\n" + m_tempEquipment.type;
            yText.text = "Equip\n" + m_tempEquipment.type;
            aText.text = " ";
            MusicManager.instance.PlayOpenChest();
        }
        
    }

    private void OnBButton()
    {
        if (canDash)
        {
            dashPos = transform.position + (transform.forward * dashDist);

            RaycastHit info;

            if (Physics.Raycast(transform.position, transform.forward, out info, dashDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                dashPos = info.point - (transform.forward * 1.5f);
            }
            else if(Physics.Raycast(transform.position + transform.right, transform.forward, out info, dashDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                dashPos = info.point - (transform.forward * 1.5f);
            }
            else if (Physics.Raycast(transform.position - transform.right, transform.forward, out info, dashDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
            {
                dashPos = info.point - (transform.forward * 1.5f);
            }

            isDashing = true;
            canDash = false;
            dashTimer = 0.0f;
            bText.text = " ";
            MusicManager.instance.PlayDash();
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
                else if(Physics.Raycast(transform.position - transform.right, transform.forward, 2, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    m_yEquipment.transform.position = transform.position - transform.forward * 2;
                }
                else if (Physics.Raycast(transform.position + transform.right, transform.forward, 2, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    m_yEquipment.transform.position = transform.position - transform.forward * 2;
                }
                else
                {
                    m_yEquipment.transform.position = transform.position + transform.forward * 2;
                }
                m_yEquipment.SetOwnerAndSlot(null, ' ');
            }

            m_yEquipment = m_tempEquipment;
            m_tempEquipment = null;
            yText.text = m_yEquipment.type;

            if(m_xEquipment)
            {
               xText.text =  m_xEquipment.type;
            }
            else
            {
                xText.text = " ";
            }

            m_shouldCheckToEquip = false;
            m_yEquipment.MoveToInventory();
        }
        else if (m_yEquipment != null && !m_holdshield && !holding && !m_swingsword)
        {
            m_yEquipment.TriggerAbitily(this.gameObject);
            yText.text = " ";
            m_yEquipment.SetOwnerAndSlot(this,'y');
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
                else if (Physics.Raycast(transform.position - transform.right, transform.forward, 2, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    m_xEquipment.transform.position = transform.position - transform.forward * 2;
                }
                else if (Physics.Raycast(transform.position + transform.right, transform.forward, 2, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    m_xEquipment.transform.position = transform.position - transform.forward * 2;
                }
                else
                {
                    m_xEquipment.transform.position = transform.position + transform.forward * 2;
                }
                m_xEquipment.SetOwnerAndSlot(null, ' ');
            }

            m_xEquipment = m_tempEquipment;
            m_tempEquipment = null;
            xText.text = m_xEquipment.type;

            if (m_yEquipment)
            {
                yText.text = m_yEquipment.type;
            }
            else
            {
                yText.text = " ";
            }

            m_shouldCheckToEquip = false;
            m_xEquipment.MoveToInventory();
        }
        else if (m_xEquipment != null && !m_holdshield && !holding && !m_swingsword)
        {
            m_xEquipment.TriggerAbitily(this.gameObject);
            xText.text = " ";
            m_xEquipment.SetOwnerAndSlot(this, 'x');
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
            xText.text = " ";
            yText.text = " ";
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

        if(lastSwordRot == m_RightHand.transform.localRotation)
        {
            m_swingRot = m_finalSwordRot;
        }

        if (((m_RightHand.transform.localRotation.eulerAngles.y <= m_finalSwordRot && m_RightHand.transform.localRotation.eulerAngles.y >= m_finalSwordRot - 1) && m_swingRot == m_finalSwordRot))
        {
            m_RightHand.SetActive(false);
            m_swingRot = m_originalSwordRot;

            m_swingsword = false;
            m_sword.GetComponent<Sword>().SetBlocked(false);
            m_RightHand.transform.localRotation = Quaternion.Euler(m_RightHand.transform.localRotation.eulerAngles.x, m_originalSwordRot, m_RightHand.transform.localRotation.eulerAngles.z);
        }

        lastSwordRot = m_RightHand.transform.localRotation;

    }

    private void HoldShield()
    {
        m_LeftHand.transform.localRotation = Quaternion.Lerp(m_LeftHand.transform.localRotation, Quaternion.Euler(m_LeftHand.transform.localRotation.eulerAngles.x, m_shieldRot, m_LeftHand.transform.localRotation.eulerAngles.z), Time.deltaTime * m_shieldSpeed);

        if (((m_LeftHand.transform.localRotation.eulerAngles.y <= m_originalshieldRot + 1 && m_LeftHand.transform.localRotation.eulerAngles.y >= m_originalshieldRot) && m_shieldRot == m_originalshieldRot))
        {
            m_holdshield = false;
            CanNoLongerEquip();
        }
    }

    public void TakeDamage(int i_damage)
    {
        if (!isImmune)
        {
            //health -= i_damage;
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
        healthText.text = health.ToString();

        if(health <= 25)
        {
            if(!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void SetCouldHold(GameObject i_pickup)
    {
        DisplayA("Pick\nUp");
        couldHold = i_pickup;
    }

    public void RemoveCould(GameObject i_pickup)
    {
        if (couldHold != null && couldHold.Equals(i_pickup))
        {
            aText.text = " ";
            couldHold = null;
        }
    }

    public GameObject GetRightHand()
    {
        return m_RightHand;
    }

    public void SteppedOnSpike()
    {
        slowTimer = 0.0f;

        if (!isSlowed)
        {
            isSlowed = true;
            m_walkSpeed /= 2;
            dashSpeed /= 2;
        }
        TakeDamage(5);
    }

    private void CheckSlowed()
    {
        if(slowTimer >= slowDelay)
        {
            m_walkSpeed *= 2;
            dashSpeed *= 2;
            isSlowed = false;
        }
        else
        {
            slowTimer += Time.deltaTime;
        }
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

    private void CheckHealth()
    {
        if(health <= 0)
        {       
            GameStateManager.instance.CheckGameOver(m_playerID);
            playerHUD.SetActive(false);
            roomID = -1;
            MusicManager.instance.PlayPlayerKill();
            Destroy(this.gameObject);
        }
    }

    public void KnockBack(Vector3 direction, float knockBackDist)
    {
        dashPos = transform.position + (direction * knockBackDist);

        RaycastHit info;

        if (Physics.Raycast(transform.position, direction, out info, knockBackDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            dashPos = info.point - (direction * 1.5f);
        }
        else if (Physics.Raycast(transform.position - transform.right, direction, out info, knockBackDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            dashPos = info.point - (direction * 1.5f);
        }
        else if (Physics.Raycast(transform.position + transform.right, direction, out info, knockBackDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            dashPos = info.point - (direction * 1.5f);
        }

        isDashing = true;
    }

    public void DisplayY()
    {
        if(yText.text.Equals(" "))
        {
            yText.text = m_yEquipment.type;
        }
    }
    public void DisplayX()
    {
        if (xText.text.Equals(" "))
        {
            xText.text = m_xEquipment.type;
        }
    }
    public void DisplayB()
    {
        if (bText.text.Equals(" "))
        {
            bText.text = "Dash";
        }
    }

    public void DisplayA(string i_text)
    {
        aText.text = i_text;
    }

    public void HealPlayer(int heal)
    {
        health += heal;

        if(health > 100)
        {
            health = 100;
        }

        UpdateHealthIndicator();
    }

    public void CanNoLongerEquip()
    {
        if (m_yEquipment)
        {
            yText.text = m_yEquipment.type;
        }
        else
        {
            yText.text = " ";
        }

        if (m_xEquipment)
        {
            xText.text = m_xEquipment.type;
        }
        else
        {
            xText.text = " ";
        }
    }
}
