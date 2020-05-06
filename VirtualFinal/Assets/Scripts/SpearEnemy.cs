using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpearEnemy : Enemy
{
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private GameObject spearPrefab;
    GameObject currSpear = null;
    private float throwTimer = 0.0f;
    [SerializeField] float throwRate = 2.0f;
    private bool canThrow = true;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }  

    // Update is called once per frame
    new void Update()
    {
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
                if (sightRange.isSet && sightRange.players.Count != 0)
                {
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

                    if (transform.rotation.eulerAngles.y <= yRot + 2.0f || transform.rotation.eulerAngles.y >= yRot - 2.0f)
                    {
                        ThrowSpear();
                    }
                }

                if (sightRange.players.Count == 0)
                {
                    canThrow = false;
                    throwTimer = throwRate / 2;
                }

                if (closestPlayer)
                {
                    lastknownLocation = closestPlayer.transform.position;
                }
            }
        }

        base.Update();
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
}
