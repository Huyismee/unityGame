using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    [Header("Ammo")] 
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")] 
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")] 
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;


    public GameObject middleDot;

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

    private void Update()
    {
        Weapon activeWeapon = WeaponManger.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft}";
            totalAmmoUI.text = $"{WeaponManger.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);
            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManger.Instance.lethalCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }
        if (WeaponManger.Instance.tacticalCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }

    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                return Resources.Load("Pistol1911_Weapon") as Sprite;
            case Weapon.WeaponModel.M4:
                return Resources.Load("M4_Weapon") as Sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                return Resources.Load("Pistol_Ammo") as Sprite;
            case Weapon.WeaponModel.M4:
                return Resources.Load("Rifle_Ammo") as Sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManger.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManger.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        //this never happen, but we need to return something
        return null;
    }

    public void UpdateThrowablesUI()
    {
        lethalAmountUI.text = $"{WeaponManger.Instance.lethalCount}";
        tacticalAmountUI.text = $"{WeaponManger.Instance.tacticalCount}";
        switch (WeaponManger.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite =Resources.Load("Grenade") as Sprite;
                break;
        }
        switch (WeaponManger.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = Resources.Load("Smoke_Grenade") as Sprite;
                break;
        }
    }
}
