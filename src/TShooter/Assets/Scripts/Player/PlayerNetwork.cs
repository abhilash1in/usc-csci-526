using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Player))]

public class PlayerNetwork : NetworkBehaviour
{
    Player player;
    PlayerAnimation PlayerAnimation;

    NetworkState state;
    NetworkState lastSendState;
    NetworkState lastSendRpcState;
    NetworkState lastReceivedState;
    NetworkState lastReceivedRpcState;

    List<NetworkState> predictedStates;

    [System.Serializable]
    public partial class NetworkState: InputController.InputState
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float AimTargetX;
        public float AimTargetY;
        public float AimTargetZ;
        public float RotationAngleY;
        public float TimeStamp;

    }

    void Start()
    {
        player = GetComponent<Player>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
        predictedStates = new List<NetworkState>();

        state = new NetworkState(); 

        if (isLocalPlayer)
            player.SetAsLocalPlayer();
    }

    private NetworkState CollectInput()
    {
        var state = new NetworkState
        {
            Fire1 = GameManager.Instance.InputController.Fire1,
            Fire2 = GameManager.Instance.InputController.Fire2,
            Horizontal = GameManager.Instance.InputController.Horizontal,
            Vertical = GameManager.Instance.InputController.Vertical,
            IsCrouching = GameManager.Instance.InputController.IsCrouching,
            IsSprinting = GameManager.Instance.InputController.IsSprinting,
            IsWalking = GameManager.Instance.InputController.IsWalking,
            Reload = GameManager.Instance.InputController.Reload,
            RotationAngleY = transform.rotation.eulerAngles.y,
            TimeStamp = Time.time

        };

        if(state.Fire1)
        {
            Vector3 shootingSolution = player.PlayerShoot.ActiveWeapon.GetImapctPoint();
            state.AimTargetX = shootingSolution.x;
            state.AimTargetY = shootingSolution.y;
            state.AimTargetZ = shootingSolution.z;
        }

        return state;
    }

    private void Update()
    {
        if(isLocalPlayer)
        {
            state = CollectInput();
            player.SetInputController(state);
            player.Move(state.Horizontal, state.Vertical);
        }

        if (lastReceivedState == null)
            return;

        UpdateState();

    }

    void UpdateState()
    {

        Vector3 serverPosition = new Vector3(lastReceivedState.PositionX, lastReceivedState.PositionY, lastReceivedState.PositionZ);

        if(isLocalPlayer && !isServer)
        {
            predictedStates.RemoveAll(x => x.TimeStamp < lastReceivedState.TimeStamp);
            var predictedState = (predictedStates.Where(x => x.TimeStamp == lastReceivedState.TimeStamp)).First();

            Vector3 predictedPosition = new Vector3(predictedState.PositionX, predictedState.PositionY, predictedState.PositionZ);
            float positionDifferenceFromServer = Vector3.Distance(predictedPosition, serverPosition);

            if(positionDifferenceFromServer > 0.3f)
            {
                transform.position = Vector3.Lerp(transform.position, serverPosition, player.runSpeed * Time.deltaTime);
            }

        }

        if (!isLocalPlayer)
        {

            PlayerAnimation.Vertical = lastReceivedState.Vertical;
            PlayerAnimation.Horizontal = lastReceivedState.Horizontal;
            PlayerAnimation.IsWalking = lastReceivedState.IsWalking;
            PlayerAnimation.isAiming = lastReceivedState.IsAiming;
            PlayerAnimation.IsSprinting = lastReceivedState.IsSprinting;
            PlayerAnimation.IsCrouching = lastReceivedState.IsCrouching;
            PlayerAnimation.AimAngle = lastReceivedState.AimAngle;

            player.SetInputController(lastReceivedState);
            player.setInputState(lastReceivedState);

            Vector3 shootingSolution = new Vector3(lastReceivedState.AimTargetX, lastReceivedState.AimTargetY, lastReceivedState.AimTargetZ);
            if (shootingSolution != Vector3.zero)
                player.PlayerShoot.ActiveWeapon.SetAimPoint(shootingSolution);

            transform.rotation = Quaternion.Euler(transform.transform.rotation.eulerAngles.x, lastReceivedState.RotationAngleY, transform.transform.rotation.eulerAngles.z);
            player.Move(lastReceivedState.Horizontal, lastReceivedState.Vertical);

            if(!isServer)
            {
                float positionDifferenceFromServer = Vector3.Distance(transform.position, serverPosition);
                if(positionDifferenceFromServer > 0.3f)
                {
                    transform.position = Vector3.Lerp(transform.position, serverPosition, player.runSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            if(isInputStateDirty(state, lastSendState))
            {
                lastSendState = state;
                Cmd_HandleInput(SerializeState(lastSendState));

                state.PositionX = transform.position.x;
                state.PositionY = transform.position.y;
                state.PositionZ = transform.position.z;

                predictedStates.Add(state);
            }
        }

        if (isServer && lastReceivedRpcState != null)
        {
            NetworkState stateSolution = new NetworkState
            {
                PositionX = transform.position.x,
                PositionY = transform.position.y,
                PositionZ = transform.position.z,
                Horizontal = lastReceivedRpcState.Horizontal,
                Vertical = lastReceivedRpcState.Vertical,
                AimAngle = lastReceivedRpcState.AimAngle,
                Fire1 = lastReceivedRpcState.Fire1,
                Fire2 = lastReceivedRpcState.Fire2,
                IsAiming = lastReceivedRpcState.IsAiming,
                IsCrouching = lastReceivedRpcState.IsCrouching,
                IsSprinting = lastReceivedRpcState.IsSprinting,
                IsWalking = lastReceivedRpcState.IsWalking,
                Reload = lastReceivedRpcState.Reload,
                RotationAngleY = lastReceivedRpcState.RotationAngleY,
                TimeStamp = lastReceivedRpcState.TimeStamp,
                AimTargetX = lastReceivedRpcState.AimTargetX,
                AimTargetY = lastReceivedRpcState.AimTargetY,
                AimTargetZ = lastReceivedRpcState.AimTargetZ
                
            };

            if (isInputStateDirty(stateSolution, lastSendRpcState))
            { 
                lastSendRpcState = stateSolution;
                Rpc_HandleStateSolution(SerializeState(lastSendRpcState));
            }
        }
    }

    // Network Code

    [Command]
    void Cmd_HandleInput(byte[] data)
    {
        lastReceivedRpcState = DeserializeState(data);
    }

    [ClientRpc]
    void Rpc_HandleStateSolution(byte[] data)
    {
        lastReceivedState = DeserializeState(data);
    }


    bool isInputStateDirty(NetworkState a, NetworkState b)
    {
        if (b == null)
            return true;

        return a.AimAngle != b.AimAngle ||
            a.Fire1 != b.Fire1 ||
            a.Fire2 != b.Fire2 ||
            a.Horizontal != b.Horizontal ||
            a.Vertical != b.Vertical ||
            a.IsAiming != b.IsAiming ||
            a.IsCrouching != b.IsCrouching ||
            a.IsSprinting != b.IsSprinting ||
            a.IsWalking != b.IsWalking ||
            a.Reload != b.Reload ||
            a.RotationAngleY != b.RotationAngleY; 

    }

    private BinaryFormatter bf = new BinaryFormatter();

    private byte[] SerializeState(NetworkState state)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            bf.Serialize(stream, state);
            return stream.ToArray();
        }
    }

    private NetworkState DeserializeState(byte[] bytes)
    {
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            return (NetworkState)bf.Deserialize(stream);
        }
    }

}
