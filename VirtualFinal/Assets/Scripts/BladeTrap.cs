using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrap : Switch
{
    override public void HitSwitch()
    {
        foreach (GameObject hole in objects)
        {
            if (hole.CompareTag("Blade"))
            {
                hole.GetComponent<Blade>().Extend();
            }
        }
        MusicManager.instance.PlayHitSwitch();
    }
}
