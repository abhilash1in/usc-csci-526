using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEventReceiver : MonoBehaviour, IPointerDownHandler
{
    public GameObject playerGO;
    public PlayerShoot playerShoot;

    void Awake()
    {
        addPhysicsRaycaster();
        playerGO = GameObject.Find("Player");
        playerShoot = playerGO.GetComponent<PlayerShoot>();
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
        if (playerShoot != null && playerShoot.ActiveWeapon != null)
            playerShoot.ActiveWeapon.Reload();
    }
}
