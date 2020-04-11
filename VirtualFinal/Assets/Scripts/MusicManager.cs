using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager m_instance;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource bGSource;
    [SerializeField] private AudioClip ShieldStrike;
    [SerializeField] private AudioClip SwordSwing;
    [SerializeField] private AudioClip BombExplosion;
    [SerializeField] private AudioClip DamagedPlayer;
    [SerializeField] private AudioClip SpearStick;

    public static MusicManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("MusicManager").AddComponent<MusicManager>();
            }
            return m_instance;
        }
    }

    //This allows each scene to have its own music manager. When a new scene is loaded the previous music manager gets replace to keep scene specific references
    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayStrike()
    {
        source.PlayOneShot(ShieldStrike);
    }

    public void PlaySwing()
    {
        source.PlayOneShot(SwordSwing);
    }

    public void PlayExplosion()
    {
        source.PlayOneShot(BombExplosion);
    }

    public void PlayDamagedPlayer()
    {
        source.PlayOneShot(DamagedPlayer);
    }

    public void PlaySpearStick()
    {
        source.PlayOneShot(SpearStick);
    }
}
