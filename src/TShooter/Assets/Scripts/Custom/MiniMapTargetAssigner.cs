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
        print("Instance_OnLocalPlayerJoined");
        localPlayerTransform = player.transform;
        miniMapController.target = localPlayerTransform;
    }


    // Update is called once per frame
    void Update()
    {
        //if (miniMapController == null)
        //{
        //    print("miniMapController null");
        //    return;
        //}

        //if (localPlayerTransform == null)
        //{
        //    print("localPlayerTransform null");
        //    localPlayerTransform = GameManager.Instance.LocalPLayer.transform;

        //    if (localPlayerTransform == null)
        //    {
        //        print("localPlayerTransform still null");
        //        return;
        //    }
        //}
        //miniMapController.target = localPlayerTransform;
    }
}
