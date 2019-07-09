using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapTargetAssigner : MonoBehaviour
{
    Transform localPlayerTransform;
    MiniMapController miniMapController;
    bool MiniMapTargetAssigned;

    private void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
        miniMapController = GetComponent<MiniMapController>();
    }

    void Instance_OnLocalPlayerJoined(Player player)
    {
        localPlayerTransform = player.transform;
        miniMapController.target = localPlayerTransform;
    }
}
