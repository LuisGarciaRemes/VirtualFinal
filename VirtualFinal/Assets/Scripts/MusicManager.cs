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
    [SerializeField] private AudioClip FallingNoise;
    [SerializeField] private AudioClip BlobBounce;
    [SerializeField] private AudioClip Splat;
    [SerializeField] private AudioClip Dash;
    [SerializeField] private AudioClip Throw;
    [SerializeField] private AudioClip ShootSlime;
    [SerializeField] private AudioClip StairsUp;
    [SerializeField] private AudioClip StairsDown;
    [SerializeField] private AudioClip Thud;
    [SerializeField] private AudioClip Lift;
    [SerializeField] private AudioClip FireballShoot;
    [SerializeField] private AudioClip FireballHit;

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

    public void PlayFalling()
    {
        source.PlayOneShot(FallingNoise);
    }

    public void PlayBlobBounce()
    {
        source.PlayOneShot(BlobBounce);
    }

    public void PlaySplat()
    {
        source.PlayOneShot(Splat);
    }

    public void PlayDash()
    {
        source.PlayOneShot(Dash);
    }

    public void PlayThrow()
    {
        source.PlayOneShot(Throw);
    }

    public void PlayShootSlime()
    {
        source.PlayOneShot(ShootSlime);
    }

    public void PlayStairsUp()
    {
        source.PlayOneShot(StairsUp);
    }

    public void PlayStairsDown()
    {
        source.PlayOneShot(StairsDown);
    }

    public void PlayThud()
    {
        source.PlayOneShot(Thud);
    }

    public void PlayLift()
    {
        source.PlayOneShot(Lift);
    }

    public void PlayFireballShoot()
    {
        source.PlayOneShot(FireballShoot);
    }

    public void PlayFireballHit()
    {
        source.PlayOneShot(FireballHit);
    }
}
