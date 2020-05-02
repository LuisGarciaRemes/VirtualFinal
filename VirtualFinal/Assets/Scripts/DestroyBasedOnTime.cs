using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBasedOnTime : MonoBehaviour
{

    [SerializeField] private float lifeTime = 3;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= lifeTime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
