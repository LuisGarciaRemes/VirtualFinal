using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] public GameObject[] objects;

    internal bool toggle = false;

    virtual public void HitSwitch()
    {
        if(!toggle)
        {
            foreach(GameObject hole in objects)
            {
                hole.SetActive(false);
                toggle = true;
            }
        }
        else
        {
            foreach (GameObject hole in objects)
            {
                hole.SetActive(true);
                toggle = false;
            }
        }
    }

}
