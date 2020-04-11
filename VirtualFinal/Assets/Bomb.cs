using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private float timer;
    [SerializeField] float delay;
    [SerializeField] float flashOffset;
    [SerializeField] GameObject blastSphere;
    private int tick;
    private Color originalColor;

    // Update is called once per frame

    private void Start()
    {
        originalColor = transform.gameObject.GetComponent<MeshRenderer>().material.color;
    }

    void Update()
    {
        if(timer >= delay)
        {
            blastSphere.SetActive(true);
            MusicManager.instance.PlayExplosion();
            Destroy(this);
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (timer >= delay - flashOffset)
        {
            if (tick % 5 == 0)
            {
                if (transform.gameObject.GetComponent<MeshRenderer>().material.color == originalColor)
                {
                    transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    transform.gameObject.GetComponent<MeshRenderer>().material.color = originalColor;
                }
            }
            else
            {
                tick++;
            }
        }
    }
}
