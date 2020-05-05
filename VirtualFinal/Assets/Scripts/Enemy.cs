using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private int currHealth;
    [SerializeField] private int maxHealth;
    internal bool isDead = false;
    internal float distanceToHeaven = 200;
    internal bool isSlowed = false;
    [SerializeField] internal float speed = 20;

    private float slowedTimer = 0.0f;
    [SerializeField] private float slowTime = 2.0f;


    private float immuneTimer = 0.0f;
    [SerializeField] private float immuneDelay = 1.0f;
    private int immuneTick = 0;
    private bool isImmune = false;

    [SerializeField] private SkinnedMeshRenderer meshToFlash;

    internal bool isKnockedBack = false;
    internal Vector3 knockBackPos;

    internal NavMeshAgent navMeshAgent = null;

    private float respawnDelay = 120;
    private float respawnTimer = 0.0f;

    internal int roomID = 0;
    [SerializeField] internal GameObject room;
    [SerializeField] internal SightRange sightRange;

    internal float wanderTimer = 0.0f;
    internal float wanderDelay = 0.0f;
    [SerializeField] internal float min = 0.5f;
    [SerializeField] internal float max = 2.0f;
    internal GameObject closestPlayer = null;
    internal Vector3 destination;
    internal Vector3 lastknownLocation;
    internal bool shouldWander = true;

    public void Start()
    {
        currHealth = maxHealth;

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("Enemy does not have navmeshagent");
        }

        navMeshAgent.speed = speed;
    }

    public void Update()
    {
        if (!isDead)
        {
            if (!sightRange.isSet)
            {
                roomID = GameStateManager.instance.listOfRooms.IndexOf(room);
                sightRange.SetRoomId(roomID);
            }

            if (currHealth <= 0)
            {
                Die();
            }

            if (isSlowed)
            {
                CheckSlowed();
            }

            if (isImmune)
            {
                FlashWhileImmune();
            }

            if (isKnockedBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, knockBackPos, Time.deltaTime * 20.0f);

                if ((knockBackPos - transform.position).magnitude <= .1)
                {
                    isKnockedBack = false;
                    navMeshAgent.enabled = true;
                }
            }
        }
        else
        {
            if(respawnTimer >= respawnDelay)
            {
                bool shouldRespawn = true;

                foreach(PlayerController player in GameStateManager.instance.listOfPlayers)
                {
                    if(player.roomID == roomID)
                    {
                        shouldRespawn = false;
                    }
                }

                if(shouldRespawn)
                {
                    Revive();
                    respawnTimer = 0.0f;
                }
            }
            else
            {
                respawnTimer += Time.deltaTime;
            }
        }
    }

    public void Die()
    {
        GameStateManager.instance.SpawnHeart(this.transform.position);
        transform.position += transform.up * distanceToHeaven;
        isDead = true;
        navMeshAgent.enabled = false;
    }

    public void TakeDamage(int i_damage)
    {
        if (!isImmune)
        {
            currHealth -= i_damage;
            BecomeImmune();
            OnTakeDamage();
        }
    }

    public void Revive()
    {
        transform.position -= transform.up*distanceToHeaven;
        isDead = false;
        currHealth = maxHealth;
        navMeshAgent.enabled = true;
    }

    virtual public void OnTakeDamage()
    {

    }

    public void SteppedOnSpike()
    {
        if (!isSlowed)
        {
            slowedTimer = 0.0f;
            isSlowed = true;
            speed /= 2;
            navMeshAgent.speed = speed;
        }
        TakeDamage(5);
    }

    private void CheckSlowed()
    {
        if (slowedTimer >= slowTime)
        {
            speed *= 2;
            isSlowed = false;
            navMeshAgent.speed = speed;
        }
        else
        {
            slowedTimer += Time.deltaTime;
        }
    }

    public void BecomeImmune()
    {
        isImmune = true;
        immuneTimer = 0.0f;
    }

    private void FlashWhileImmune()
    {
        if (immuneTimer >= immuneDelay)
        {
            meshToFlash.enabled = true;
            isImmune = false;
            immuneTick = 0;
        }
        else
        {
            immuneTimer += Time.deltaTime;
            immuneTick++;

            if (immuneTick % 15 == 0)
            {
                if (meshToFlash.enabled)
                {
                    meshToFlash.enabled = false;
                }
                else
                {
                    meshToFlash.enabled = true;
                }
            }
        }
    }

    virtual public void MoveAwayFromDoor()
    {

    }

    public void KnockBack(Vector3 direction, float knockBackDist)
    {
        knockBackPos = transform.position + (direction * knockBackDist);

        RaycastHit info;

        if (Physics.Raycast(transform.position, direction, out info, knockBackDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            knockBackPos = info.point - (direction * 1.5f);
        }
        else if (Physics.Raycast(transform.position - transform.right, direction, out info, knockBackDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            knockBackPos = info.point - (direction * 1.5f);
        }
        else if (Physics.Raycast(transform.position + transform.right, direction, out info, knockBackDist + 0.5f, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
        {
            knockBackPos = info.point - (direction * 1.5f);
        }

        isKnockedBack = true;

        navMeshAgent.enabled = false;
    }
}
