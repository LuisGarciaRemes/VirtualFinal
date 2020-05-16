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

    [SerializeField] internal float minDistance = 5.0f;
    [SerializeField] internal float maxDistance = 10.0f;

    internal float yRot;
    [SerializeField] internal float rotateSpeed = 10;
    internal bool stopRotating = false;
    private Vector3 ogPos;

    public void Start()
    {
        currHealth = maxHealth;

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("Enemy does not have navmeshagent");
        }

        navMeshAgent.speed = speed;
        ogPos = transform.position;
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

            if (shouldWander)
            {

                if (wanderTimer >= wanderDelay)
                {
                    wanderTimer = 0.0f;
                    wanderDelay = UnityEngine.Random.Range(min, max);
                    destination = RandomNavSphere(transform.position, 10.0f, -1);
                    if (navMeshAgent.enabled)
                    {
                        navMeshAgent.SetDestination(destination);
                    }
                }
                else
                {
                    wanderTimer += Time.deltaTime;
                }

                if (sightRange.isSet && sightRange.players.Count != 0)
                {
                    shouldWander = false;
                    wanderTimer = 0.0f;
                    wanderDelay = 0.0f;
                    navMeshAgent.updateRotation = false;
                    destination = transform.position;
                    if (navMeshAgent.enabled)
                    {
                        navMeshAgent.SetDestination(destination);
                    }
                }
            }
            else
            {
                closestPlayer = sightRange.FindNearestPlayer();
                if (closestPlayer && closestPlayer.GetComponent<PlayerController>().roomID == roomID)
                {
                    Vector3 direction = closestPlayer.transform.position - transform.position;
                    yRot = Mathf.Atan2(direction.normalized.x, direction.normalized.z) * 57.2958f;

                    if (!stopRotating)
                    {
                        transform.rotation = Quaternion.Euler(0.0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, yRot, Time.deltaTime * rotateSpeed), 0.0f);
                    }

                    if (direction.magnitude <= minDistance)
                    {
                        destination = transform.position - direction.normalized;
                        if (navMeshAgent.enabled)
                        {
                            navMeshAgent.SetDestination(destination);
                        }
                    }
                    else if (direction.magnitude >= maxDistance)
                    {
                        destination = transform.position + direction.normalized;
                        if (navMeshAgent.enabled)
                        {
                            navMeshAgent.SetDestination(destination);
                        }
                    }
                }

                if (sightRange.players.Count == 0)
                {
                    shouldWander = true;
                    navMeshAgent.updateRotation = true;
                    wanderDelay = UnityEngine.Random.Range(min, max);
                    wanderTimer = 0.0f;
                    destination = RandomNavSphere(lastknownLocation, 2.0f, -1);
                    if (navMeshAgent.enabled)
                    {
                        navMeshAgent.SetDestination(destination);
                    }
                    closestPlayer = null;
                }

                if (closestPlayer)
                {
                    lastknownLocation = closestPlayer.transform.position;
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
                    if(player != null && player.roomID == roomID)
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
        MusicManager.instance.PlayEnemyKill();
    }

    public void TakeDamage(int i_damage)
    {
        if (!isImmune)
        {
            currHealth -= i_damage;
            BecomeImmune();
            OnTakeDamage();
            MusicManager.instance.PlayEnemyDamage();
        }
    }

    public void Revive()
    {
        transform.position = ogPos;
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


    public void MoveAwayFromDoor()
    {
        destination = RandomNavSphere(room.transform.position, 10.0f, -1);
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(destination);
        }
        wanderDelay = UnityEngine.Random.Range(min, max);
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

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
