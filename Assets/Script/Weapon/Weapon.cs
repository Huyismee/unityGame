using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool isActiveWeapon;
    public int weaponDamage;

    //shooting
    [Header("Shooting")]

    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    //burst
    [Header("Burst")]
    public int bulletPerBurst;
    public int burstBulletsLeft;

    //spread
    [Header("Spread")]
    private float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    //Bullet
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    //Loading
    [Header("Reload")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;


    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private bool isADS;
    public enum WeaponModel
    {
        Pistol1911,
        M4
    }

    public WeaponModel thisWeaponModel;



    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode CurShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
    }

    // Update is called once per frame
    void Update()
    {

        if (isActiveWeapon)
        {

            foreach (Transform child in transform)
            {
                foreach (Transform nchild in child)
                {
                    nchild.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
                }
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }

            transform.gameObject.layer = LayerMask.NameToLayer("WeaponRender");

            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }



            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazine1911.Play();
            }
            if (CurShootingMode == ShootingMode.Auto)
            {
                //holding down left mouse button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (CurShootingMode == ShootingMode.Single || CurShootingMode == ShootingMode.Burst)
            {
                //clicking left mouse button once
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0 && !isReloading)
            {
                burstBulletsLeft = bulletPerBurst;
                FireWeapon();
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            transform.gameObject.layer = LayerMask.NameToLayer("Default");


        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {

        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        if (!isADS)
        {
            animator.SetTrigger("RECOIL");
        }
        else
        {
            animator.SetTrigger("ADS_Recoil");
        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        //pointing the  bullet to face the  shooting direction
        bullet.transform.forward = shootingDirection;

        //Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Destroy bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //Checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //burst mode
        if (CurShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if (WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel) + bulletsLeft > magazineSize)
        {
            WeaponManger.Instance.DecreaseTotalAmmo(magazineSize - bulletsLeft, thisWeaponModel);
            bulletsLeft = magazineSize;
        }
        else
        {
            bulletsLeft += WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManger.Instance.DecreaseTotalAmmo(WeaponManger.Instance.CheckAmmoLeftFor(thisWeaponModel), thisWeaponModel);

        }
        isReloading = false;

    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //return the shooting direction and spread
        return direction + new Vector3(0, y, z);


    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float f)
    {
        yield return new WaitForSeconds(f);
        Destroy(bullet);
    }
}
