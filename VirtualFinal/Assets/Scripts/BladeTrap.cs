using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrap : Switch
{
    override public void HitSwitch()
    {
        foreach (GameObject hole in objects)
        {
            hole.GetComponent<Blade>().Extend();
        }
    }
}
