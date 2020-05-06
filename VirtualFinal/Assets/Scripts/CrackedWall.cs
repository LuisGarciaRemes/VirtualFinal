using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    public void BreakWall()
    {
        MusicManager.instance.PlaySecret();
        Destroy(this.gameObject);
    }
}
