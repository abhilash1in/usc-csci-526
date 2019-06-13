using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [System.Serializable]
    public class CameraRig
    {
        public Vector3 CameraOffset;
        public float CrouchHeight;
        public float Damping;
    }

    // 0.7, 0.8, -7
    // 5
    [SerializeField] CameraRig defaultCamera;
    [SerializeField] CameraRig aimCamera;



    Transform cameraLookTarget;
    Player localPlayer;

    void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
    }

    void HandleOnLocalPlayerJoined(Player player)
    {
        localPlayer = player;
        cameraLookTarget = localPlayer.transform.Find("CameraLookTarget");

        if(cameraLookTarget == null)
        {
            cameraLookTarget = localPlayer.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (localPlayer == null)
            return;

        CameraRig cameraRig = defaultCamera;

        if(localPlayer.PlayerState.WeaponState == 
            PlayerState.EWeaponState.AIMING || 
            localPlayer.PlayerState.WeaponState == 
            PlayerState.EWeaponState.AIMEDFIRING)
        {
            cameraRig = aimCamera;
        }

        float targetHeight = cameraRig.CameraOffset.y + 
            (localPlayer.PlayerState.MoveState == PlayerState.EMoveState.CROUCHING ? cameraRig .CrouchHeight : 0);

        Vector3 targetPosition = cameraLookTarget.position + localPlayer.transform.forward * cameraRig.CameraOffset.z +
            localPlayer.transform.up * targetHeight +
            localPlayer.transform.right * cameraRig.CameraOffset.x;

        Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);


        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraRig.Damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraRig.Damping * Time.deltaTime);
    }
}
