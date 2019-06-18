using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEventReceiver : MonoBehaviour, IPointerDownHandler
{
    public PlayerShoot playerShoot;

    private void Start()
    {
        addPhysicsRaycaster();
    }

    private PlayerShoot getShooter() {
        Player playerGO = GameManager.Instance.LocalPLayer;
        return playerGO.GetComponent<PlayerShoot>();
    }


    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        string hitGameObjectName = eventData.pointerCurrentRaycast.gameObject.name;
        if (hitGameObjectName.Equals("AmmoText"))
        {
            Reload();
        }
    }

    private void Reload()
    {
        print("Reload clicked!!!");
        playerShoot = getShooter();
        if (playerShoot != null && playerShoot.ActiveWeapon != null)
            playerShoot.ActiveWeapon.Reload();
    }
}
