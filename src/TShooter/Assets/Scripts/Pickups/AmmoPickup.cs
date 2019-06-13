using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickupItem
{

    [SerializeField] EWeaponType weaponType;
    [SerializeField] float respawnTime;
    [SerializeField] int amount;

    public override void OnPickup(Transform item)
    {
        // base.OnPickup(item);
        print(item.name);
        var playerInventory = item.GetComponentInChildren<Container>();
        GameManager.Instance.Respawner.DespawnRandom(gameObject, respawnTime);
        playerInventory.Put(weaponType.ToString(), amount);
        item.GetComponent<Player>().PlayerShoot.ActiveWeapon.Reloader.HandleOnAmmoChanged();
    }
}
