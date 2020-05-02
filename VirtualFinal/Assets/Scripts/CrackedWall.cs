using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    public void BreakWall()
    {
        Destroy(this.gameObject);
    }
}
