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
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
    }

    void HandleOnLocalPlayerJoined(Player player)
    {
        playerShoot = player.PlayerShoot;
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
        if (playerShoot == null)
            return;

        print("Reload clicked!!!");
        if (playerShoot.ActiveWeapon != null)
            playerShoot.ActiveWeapon.Reload();
    }
}
