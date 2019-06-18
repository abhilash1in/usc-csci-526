using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] Text text;
    
    PlayerShoot playerShoot;
    WeaponReloader reloader;
    int fontSize;
    int smallerFontSize;


    void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
        fontSize = text.fontSize;
        smallerFontSize = fontSize / 2;
    }

    void HandleOnLocalPlayerJoined(Player player)
    {
        playerShoot = player.PlayerShoot;
        playerShoot.OnWeaponSwitch += HandleOnWeaponSwitch;
    }

    void HandleOnWeaponSwitch(Shooter activeWeapon)
    {
        reloader = activeWeapon.Reloader;
        reloader.OnAmmoChanged += HandleOnAmmoChanged;
    }


    void HandleOnAmmoChanged()
    {
        int amountInClip = reloader.RoundsRemainingInClip;
        int amountInInventory = reloader.RoundsRemainingInInventory;

        if (amountInClip == 0 && amountInInventory == 0)
        {
            text.fontSize = smallerFontSize;
            text.text = "NO AMMO!";
            HelperMethods.ShowMessage("NO AMMO!");
        }

        else if (amountInClip == 0)
        {
            text.fontSize = smallerFontSize;
            text.text = "RELOAD!";
            HelperMethods.ShowMessage("RELOAD!");
        }
        else
        {
            text.fontSize = fontSize;
            text.text = string.Format("{0}/{1}", amountInClip, amountInInventory);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
