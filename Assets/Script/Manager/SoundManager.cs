using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource ShootingChannel;
    public AudioSource reloadingSound1911;
    public AudioSource reloadingSoundM4;

    public AudioSource emptyMagazine1911;
    public AudioSource throwablesChannel;

    public AudioClip M4Shot;
    public AudioClip P1911Shot;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;


    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    public AudioClip gameOverMusic;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case Weapon.WeaponModel.M4:
                ShootingChannel.PlayOneShot(M4Shot);
                break;

        }
    }
    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                reloadingSound1911.Play();
                break;
            case Weapon.WeaponModel.M4:
                reloadingSoundM4.Play();
                break;

        }
    }
}
