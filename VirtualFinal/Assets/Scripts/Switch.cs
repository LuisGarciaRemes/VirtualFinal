using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] public GameObject[] objects;
    public bool invert = false;
    public bool random = false;

    internal bool toggle = false;

    virtual public void HitSwitch()
    {
        if (invert)
        {
            foreach (GameObject hole in objects)
            {
                hole.SetActive(!hole.activeSelf);
            }

        }
        else if(random)
        {
            int index = Random.Range(0, objects.Length);
            objects[index].SetActive(!objects[index].activeSelf);
        }
        else
        {
            if (!toggle)
            {
                foreach (GameObject hole in objects)
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

        MusicManager.instance.PlayHitSwitch();
    }

}
