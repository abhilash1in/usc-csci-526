using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] float WeaponSwitchTime;

    Shooter[] weapons;
    Shooter activeWeapon;

    Player player;

    int currentWeaponIndex;

    bool canFire;

    Transform weaponHolster;

    public event System.Action<Shooter> OnWeaponSwitch;

    public Shooter ActiveWeapon
    {
        get
        {
            return activeWeapon;
        }
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Awake()
    {
        canFire = true;
        weaponHolster = transform.Find("Weapons");
        weapons = weaponHolster.GetComponentsInChildren<Shooter>();
        if (weapons.Length > 0)
            Equip(0);

    }

    void DeactivateWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
            weapons[i].gameObject.transform.SetParent(weaponHolster);
        }
    }

    void SwitchWeapon(int direction)
    {
        canFire = false;

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

    void Equip(int index)
    {
        DeactivateWeapons();
        canFire = true;
        activeWeapon = weapons[index];
        activeWeapon.Equip();
        activeWeapon.gameObject.SetActive(true);

        if (OnWeaponSwitch != null)
            OnWeaponSwitch(activeWeapon);
    }


    void Update()
    {
        if(!player.IsLocalPlayer)
        {
            if(player.InputState.Fire1)
            {
                ActiveWeapon.Fire();
            }
        }


        if (player.IsLocalPlayer)
        {
            if (GameManager.Instance.InputController.MouseWheelDown)
                SwitchWeapon(1);

            if (GameManager.Instance.InputController.MouseWheelUp)
                SwitchWeapon(-1);

            if (GameManager.Instance.LocalPLayer.PlayerState.MoveState == PlayerState.EMoveState.SPRINTING)
                return;

            if (!canFire)
                return;

            if (player.InputState.Fire1)
            {
                activeWeapon.SetAimPoint(activeWeapon.GetImapctPoint());
                activeWeapon.Fire();
            }
        }
    }
}
