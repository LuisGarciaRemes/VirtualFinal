using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private float timer;
    [SerializeField] float delay;
    [SerializeField] GameObject blastSphere;

    // Update is called once per frame
    void Update()
    {
        if(timer >= delay)
        {
            blastSphere.SetActive(true);
            transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            Destroy(this);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
