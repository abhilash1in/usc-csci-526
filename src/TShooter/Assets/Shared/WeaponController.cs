using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] float WeaponSwitchTime;

    [HideInInspector] public bool CanFire;

    Shooter[] weapons;

    int currentWeaponIndex;

    Transform weaponHolster;

    public event System.Action<Shooter> OnWeaponSwitch;

    Shooter m_ActiveWeapon;
    public Shooter ActiveWeapon
    {
        get
        {
            return m_ActiveWeapon;
        }
    }

    private void Awake()
    {
        CanFire = true;
        weaponHolster = transform.Find("Weapons");
        weapons = weaponHolster.GetComponentsInChildren<Shooter>();
        if (weapons.Length > 0)
            Equip(0);

    }

    internal void DeactivateWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
            weapons[i].gameObject.transform.SetParent(weaponHolster);
        }
    }

    internal void SwitchWeapon(int direction)
    {
        CanFire = false;

        currentWeaponIndex += direction;

        if (currentWeaponIndex > weapons.Length - 1)
            currentWeaponIndex = 0;

        if (currentWeaponIndex < 0)
            currentWeaponIndex = weapons.Length - 1;

        GameManager.Instance.Timer.Add(() =>
        {
            Equip(currentWeaponIndex);

        }, WeaponSwitchTime);

    }

    internal void Equip(int index)
    {
        DeactivateWeapons();
        CanFire = true;
        m_ActiveWeapon = weapons[index];
        m_ActiveWeapon.Equip();
        m_ActiveWeapon.gameObject.SetActive(true);

        if (OnWeaponSwitch != null)
            OnWeaponSwitch(m_ActiveWeapon);
    }
}
