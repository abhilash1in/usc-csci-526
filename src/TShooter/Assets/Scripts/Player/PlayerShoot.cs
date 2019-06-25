using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : WeaponController
{
    bool IsPlayerAlive;
    Player player;
    InputController.InputState inputState;


    private void Start()
    {
        IsPlayerAlive = true;
        player = GetComponent<Player>();

        //if (inputState == null)
        //{
        //    inputState = player.InputState;
        //}

        player.PlayerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    void PlayerHealth_OnDeath()
    {
        IsPlayerAlive = false;
    }

    public void SetInputControllerState(InputController.InputState state)
    {
        inputState = state;
    }


    void Update()
    {
        if(inputState == null)
        {
            print("inputState is null");
            return;
        }

        if (!player.IsLocalPlayer && IsPlayerAlive)
        {
            if (player.InputState.Fire1)
                ActiveWeapon.Fire();
        }

        if (!IsPlayerAlive || GameManager.Instance.IsPaused)
            return;

        if (player.IsLocalPlayer)
        {
            if (GameManager.Instance.InputController.MouseWheelDown)
                SwitchWeapon(1);

            if (GameManager.Instance.InputController.MouseWheelUp)
                SwitchWeapon(-1);


            if (GameManager.Instance.LocalPLayer.PlayerState.MoveState == PlayerState.EMoveState.SPRINTING)
                return;

            if (!CanFire)
                return;

            if (inputState.Fire1)
            {
                ActiveWeapon.SetAimPoint(GetImpactPoint());
                ActiveWeapon.Fire();
            }


            if (inputState.Reload)
                ActiveWeapon.Reload();
        }

    }
}
