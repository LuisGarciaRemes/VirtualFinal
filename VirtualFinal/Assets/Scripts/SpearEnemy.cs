using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpearEnemy : Enemy
{
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private float minDistance = 5.0f;
    [SerializeField] private float maxDistance = 10.0f;
    GameObject currSpear = null;
    private float throwTimer = 0.0f;
    [SerializeField] float throwRate = 2.0f;
    private bool canThrow = true;
    private float yRot;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }  

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!isDead)
        {        
            if (!canThrow)
            {
                if (throwTimer >= throwRate)
                {
                    canThrow = true;
                    throwTimer = 0.0f;
                }
                else
                {
                    throwTimer += Time.deltaTime;
                }
            }

            if (shouldWander)
            {
                RaycastHit info;

                if (Physics.Raycast(transform.position, (destination - transform.position).normalized, out info, 3, 1 << 0, UnityEngine.QueryTriggerInteraction.Ignore))
                {
                    if (info.transform.gameObject.CompareTag("Wall"))
                    {
                        wanderTimer = 0.0f;
                        wanderDelay = UnityEngine.Random.Range(min, max);
                        destination = RandomNavSphere(transform.position - (destination - transform.position).normalized * 10.0f, 1.0f, -1);
                        navMeshAgent.SetDestination(destination);
                    }
                }

                if (wanderTimer >= wanderDelay)
                {
                    wanderTimer = 0.0f;
                    wanderDelay = UnityEngine.Random.Range(min, max);
                    destination = RandomNavSphere(transform.position, 10.0f, -1);
                    navMeshAgent.SetDestination(destination);
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
                    navMeshAgent.SetDestination(destination);
                    canThrow = false;
                    throwTimer = throwRate / 2;
                }
            }
            else
            {
                closestPlayer = sightRange.FindNearestPlayer();
                if (closestPlayer && closestPlayer.GetComponent<PlayerController>().roomID == roomID)
                {
                    Vector3 direction = closestPlayer.transform.position - transform.position;
                    yRot = Mathf.Atan2(direction.normalized.x, direction.normalized.z) * 57.2958f;
                    transform.rotation = Quaternion.Euler(0.0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, yRot, Time.deltaTime * 10), 0.0f);

                    if (transform.rotation.eulerAngles.y <= yRot + 2.0f || transform.rotation.eulerAngles.y >= yRot - 2.0f)
                    {
                        ThrowSpear();
                    }

                    if (direction.magnitude <= minDistance)
                    {
                        destination = transform.position - direction.normalized;
                        navMeshAgent.SetDestination(destination);
                    }
                    else if (direction.magnitude >= maxDistance)
                    {
                        destination = transform.position + direction.normalized;
                        navMeshAgent.SetDestination(destination);
                    }
                }

                if (sightRange.players.Count == 0)
                {
                    shouldWander = true;
                    navMeshAgent.updateRotation = true;
                    wanderDelay = UnityEngine.Random.Range(min, max);
                    wanderTimer = 0.0f;
                    canThrow = false;
                    throwTimer = throwRate / 2;
                    destination = RandomNavSphere(lastknownLocation, 2.0f, -1);
                    navMeshAgent.SetDestination(destination);
                    closestPlayer = null;
                }

                if (closestPlayer)
                {
                    lastknownLocation = closestPlayer.transform.position;
                }
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void ThrowSpear()
    {
        if (canThrow)
        {
            currSpear = Instantiate(spearPrefab, new Vector3(spawnLocation.transform.position.x, transform.position.y, spawnLocation.transform.position.z) + transform.forward, transform.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f));
            currSpear.GetComponentInChildren<Spear>().forward = (closestPlayer.transform.position - spawnLocation.transform.position).normalized;
            MusicManager.instance.PlayThrow();
            canThrow = false;
        }
    }

    public override void MoveAwayFromDoor()
    {
        destination = RandomNavSphere(room.transform.position, 10.0f, -1);
        navMeshAgent.SetDestination(destination);
        wanderDelay = UnityEngine.Random.Range(min, max);
    }
}
