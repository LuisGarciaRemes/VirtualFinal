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
    [SerializeField] private AudioClip HeartFill;
    [SerializeField] private AudioClip BoxBreak;
    [SerializeField] private AudioClip SpottedByEnemy;
    [SerializeField] private AudioClip EnemyKill;
    [SerializeField] private AudioClip EnemyDamage;
    [SerializeField] private AudioClip PlayerKill;
    [SerializeField] private AudioClip HitSwitch;
    [SerializeField] private AudioClip OpenChest;
    [SerializeField] private AudioClip EquipedItem;
    [SerializeField] private AudioClip Secret;

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

    public void PlayHeartFill()
    {
        source.PlayOneShot(HeartFill);
    }

    public void PlayBoxBreak()
    {
        source.PlayOneShot(BoxBreak);
    }

    public void PlaySpottedByEnemy()
    {
        source.PlayOneShot(SpottedByEnemy);
    }

    public void PlayEnemyKill()
    {
        source.PlayOneShot(EnemyKill);
    }

    public void PlayEnemyDamage()
    {
        source.PlayOneShot(EnemyDamage);
    }

    public void PlayPlayerKill()
    {
        source.PlayOneShot(PlayerKill);
    }
    public void PlayHitSwitch()
    {
        source.PlayOneShot(HitSwitch);
    }

    public void PlayOpenChest()
    {
        source.PlayOneShot(OpenChest);
    }

    public void PlayEquipedItem()
    {
        source.PlayOneShot(EquipedItem);
    }

    public void PlaySecret()
    {
        source.PlayOneShot(Secret);
    }
}
