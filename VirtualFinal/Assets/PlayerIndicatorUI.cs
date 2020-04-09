using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicatorUI : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + offSet;
        transform.rotation = Quaternion.Euler(90.0f,0.0f,0.0f);
    }
}
