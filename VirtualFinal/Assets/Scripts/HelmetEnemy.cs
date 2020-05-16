using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetEnemy : Enemy
{

    private bool canHeadbutt = true;
    [SerializeField] private GameObject headPlate;
    private float headbuttTimer = 0.0f;
    private float headbuttRate = 2.0f;

    private float rotatingTimer = 0.0f;
    [SerializeField] private float rotatingDelay = 0.5f;

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
            if (!canHeadbutt)
            {
                if (headbuttTimer >= headbuttRate)
                {
                    canHeadbutt = true;
                    headbuttTimer = 0.0f;
                }
                else
                {
                    headbuttTimer += Time.deltaTime;
                }
            }

            if(!shouldWander && canHeadbutt)
            {
                closestPlayer = sightRange.FindNearestPlayer();

                if (closestPlayer && closestPlayer.GetComponent<PlayerController>().roomID == roomID)
                {
                    Vector3 direction = closestPlayer.transform.position - transform.position;

                    yRot = Mathf.Atan2(direction.normalized.x, direction.normalized.z) * 57.2958f;

                    if ((transform.rotation.eulerAngles.y <= yRot + 2.0f || transform.rotation.eulerAngles.y >= yRot - 2.0f) && (direction.magnitude <= maxDistance))
                    {
                        Heabutt(headPlate.transform.forward,minDistance*2);
                        MusicManager.instance.PlayDash();
                        canHeadbutt = false;
                    }

                }

                if (sightRange.players.Count == 0)
                {
                    AbortHeadbutt();
                }

                if (closestPlayer)
                {
                    lastknownLocation = closestPlayer.transform.position;
                }
            }

            if(!isKnockedBack && headPlate.GetComponent<Headbutt>().isHeadbutting)
            {
                headPlate.GetComponent<Headbutt>().isHeadbutting = false;
                stopRotating = true;
            }

            if(stopRotating)
            {
                if(rotatingTimer>=rotatingDelay)
                {
                    stopRotating = false;
                    rotatingTimer = 0.0f;
                }
                else
                {
                    rotatingTimer += Time.deltaTime;
                }
            }
        }

        base.Update();
    }

    public void Heabutt(Vector3 direction, float knockBackDist)
    {
        knockBackPos = transform.position + (direction * knockBackDist);

        headPlate.GetComponent<Headbutt>().isHeadbutting = true;

        isKnockedBack = true;

        navMeshAgent.enabled = false;
    }

    public void AbortHeadbutt()
    {
        canHeadbutt = false;
        headPlate.GetComponent<Headbutt>().isHeadbutting = false;
        isKnockedBack = false;
        headbuttTimer = 0.0f;
        navMeshAgent.enabled = true;
    }

}
