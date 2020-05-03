using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamStatue : MonoBehaviour
{
    [SerializeField] private GameObject Fireball;
    [SerializeField] private GameObject Sphere;
    [SerializeField] private MeshRenderer Sight;
    private bool shouldRotate = true;
    private bool canFire = true;
    [SerializeField] private float fireRate = 1.0f;
    private float fireTimer = 0.0f;
    
    private void Update()
    {
        if (shouldRotate)
        {
            Sphere.transform.Rotate(new Vector3(0.0f, .25f, 0.0f));
        }

        if(!canFire)
        {
            if (fireTimer >= fireRate)
            {
                canFire = true;
                fireTimer = 0.0f;
            }
            else if(fireTimer >= fireRate - 0.25f && !shouldRotate)
            {
                shouldRotate = true;
                Sight.enabled = true;
            }
            else
            {
                fireTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canFire)
        {
            Vector3 direction = other.gameObject.transform.position - Sphere.transform.position;
            float angle = Mathf.Atan2(direction.x,direction.z) * 57.2958f;

            Instantiate(Fireball, Sphere.transform.position + Sphere.transform.up*1.25f, Quaternion.Euler(0.0f,angle,0.0f));
            Sight.enabled = false;
            shouldRotate = false;
            canFire = false;
        }
    }
}
