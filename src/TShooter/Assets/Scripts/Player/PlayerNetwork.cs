using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[RequireComponent(typeof(Player))]
public class PlayerNetwork : CustomNetworkBehviour
{
    [SyncVar] public bool isFrozen;

    Player player;
    PlayerMove playerMove;
    PlayerShoot playerShoot;
    PlayerAnimation playerAnimation;
    NetworkState state;
    NetworkState lastSentState;
    NetworkState lastSentRpcState;
    NetworkState lastReceivedState;
    List<NetworkState> predictedStates;
    
    [System.Serializable]
    public partial class NetworkState : InputController.InputState
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationAngleY;
        public float AimTargetX;
        public float AimTargetY;
        public float AimTargetZ;
        public float DamageTaken;
        public float Timestamp;
    }

    public void SetPlayerTeam(ETeamID teamId)
    {
        TeamID = teamId;
    }


    private void Start()
    {
        player = GetComponent<Player>();
        state = new NetworkState();
        playerMove = GetComponent<PlayerMove>();
        playerShoot = player.PlayerShoot;
        playerAnimation = GetComponent<PlayerAnimation>();
        predictedStates = new List<NetworkState>();

        player.PlayerHealth.OnDamageReceived += PlayerHealth_OnDamageReceived;

        if (isLocalPlayer)
        {
            print("setting as local player");
            player.SetAsLocalPlayer(); 
        }
    }

    void PlayerHealth_OnDamageReceived(float amount)
    {
        IsPlayerFrozen(true);
        GameManager.Instance.Timer.Add(() =>
        {
            IsPlayerFrozen(false);
        }, 5);
    }

    void IsPlayerFrozen(bool value)
    {
        isFrozen = value;
    }

    private NetworkState CollectInput()
    {
        NetworkState state = new NetworkState
        {
            CoverToggle = GameManager.Instance.InputController.CoverToggle,
            Fire1 = GameManager.Instance.InputController.Fire1,
            Fire2 = GameManager.Instance.InputController.Fire2,
            Horizontal = GameManager.Instance.InputController.Horizontal,
            Vertical = GameManager.Instance.InputController.Vertical,
            IsCrouching = GameManager.Instance.InputController.IsCrouching,
            IsSprinting = GameManager.Instance.InputController.IsSprinting,
            IsWalking = GameManager.Instance.InputController.IsWalking,
            Reload = GameManager.Instance.InputController.Reload,
            AimAngle = player.playerAim.GetAngle(),
            RotationAngleY = transform.rotation.eulerAngles.y,
            DamageTaken = player.PlayerHealth.DamageTaken,
            Timestamp = Time.time
        };

        if (state.Fire1)
        {
            Vector3 shootingSolution = player.WeaponController.GetImpactPoint();
            state.AimTargetX = shootingSolution.x;
            state.AimTargetY = shootingSolution.y;
            state.AimTargetZ = shootingSolution.z;
        }

        if (player.PlayerShoot.ActiveWeapon.Reloader.RoundsRemainingInClip == 0)
        {
            state.Fire1 = false;
        }
        return state;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            state = CollectInput();
            playerMove.SetInputControllerState(state);
            playerShoot.SetInputControllerState(state);
            if(!isFrozen)
                playerMove.Move(state.Horizontal, state.Vertical);
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
            // remove old states (if any)
            predictedStates.RemoveAll(x => x.Timestamp < lastReceivedState.Timestamp);

            // we expect the state to be there
            var predictedState = predictedStates.Where(x => x.Timestamp == lastReceivedState.Timestamp).FirstOrDefault();

            Vector3 predictedPosition = new Vector3(predictedState.PositionX, predictedState.PositionY, predictedState.PositionZ);
            float positionDifferenceFromServer = Vector3.Distance(predictedPosition, serverPosition);
            if (positionDifferenceFromServer > 1f)
                transform.position = Vector3.Lerp(transform.position, serverPosition, player.Settings.RunSpeed * Time.deltaTime); 
        }

        if (!isLocalPlayer)
        {
            if (!isFrozen)
            {
                playerAnimation.Vertical = lastReceivedState.Vertical;
                playerAnimation.Horizontal = lastReceivedState.Horizontal;
            }
            playerAnimation.IsWalking = lastReceivedState.IsWalking;
            playerAnimation.IsSprinting = lastReceivedState.IsSprinting;
            playerAnimation.IsCrouching = lastReceivedState.IsCrouching;
            playerAnimation.IsAiming = lastReceivedState.IsAiming;
            playerAnimation.IsInCover = lastReceivedState.IsInCover;
            playerAnimation.AimAngle = lastReceivedState.AimAngle;

            Vector3 shootingSolution = new Vector3(lastReceivedState.AimTargetX, lastReceivedState.AimTargetY, lastReceivedState.AimTargetZ);
            playerMove.SetInputControllerState(lastReceivedState);
            player.SetInputState(lastReceivedState);
            player.PlayerShoot.SetInputControllerState(lastReceivedState);
            player.PlayerHealth.DamageTaken = lastReceivedState.DamageTaken;


            if (shootingSolution != Vector3.zero)
            {
                player.WeaponController.ActiveWeapon.SetAimPoint(shootingSolution);
            }

            transform.rotation = Quaternion.Euler(transform.transform.rotation.eulerAngles.x, lastReceivedState.RotationAngleY, transform.transform.rotation.eulerAngles.z);
            if (!isFrozen)
                playerMove.Move(lastReceivedState.Horizontal, lastReceivedState.Vertical);

            if (!isServer)
            {
                float positionDifferenceFromServer = Vector3.Distance(transform.position, serverPosition);
                if(positionDifferenceFromServer > 1f)
                {
                    transform.position = Vector3.Lerp(transform.position, serverPosition, player.Settings.RunSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (IsInputStateDirty(state, lastSentState))
            {
                lastSentState = state;
                Cmd_HandleInput(SerializeState(lastSentState));
                state.PositionX = transform.position.x;
                state.PositionY = transform.position.y;
                state.PositionZ = transform.position.z;
                predictedStates.Add(state);
            }
        }

        // if we are the server, update the remote client
        if(isServer && lastReceivedState != null)
        {
            NetworkState stateSolution = new NetworkState
            {
                PositionX = transform.position.x,
                PositionY = transform.position.y,
                PositionZ = transform.position.z,
                Horizontal = lastReceivedState.Horizontal,
                Vertical = lastReceivedState.Vertical,
                AimAngle = lastReceivedState.AimAngle,
                CoverToggle = lastReceivedState.CoverToggle,
                Fire1 = lastReceivedState.Fire1,
                Fire2 = lastReceivedState.Fire2,
                IsAiming = lastReceivedState.IsAiming,
                IsCrouching = lastReceivedState.IsCrouching,
                IsInCover = lastReceivedState.IsInCover,
                IsWalking = lastReceivedState.IsWalking,
                Reload = lastReceivedState.Reload,
                RotationAngleY = lastReceivedState.RotationAngleY,
                AimTargetX = lastReceivedState.AimTargetX,
                AimTargetY = lastReceivedState.AimTargetY,
                AimTargetZ = lastReceivedState.AimTargetZ,
                DamageTaken = lastReceivedState.DamageTaken,
                Timestamp = lastReceivedState.Timestamp
            };

            if(IsInputStateDirty(stateSolution, lastSentRpcState))
            {
                lastSentRpcState = stateSolution;
                Rpc_HandleStateSolution(SerializeState(lastSentRpcState));
            }
        }
    }

    // called from the client and run on the server
    [Command]
    void Cmd_HandleInput(byte[] data)
    {
        lastReceivedState = DeserializeState(data);
    }

    // called on the server and run on clients
    [ClientRpc]
    void Rpc_HandleStateSolution(byte[] data)
    {
        lastReceivedState = DeserializeState(data);
    }

    bool IsInputStateDirty(NetworkState a, NetworkState b)
    {
        if (b == null)
            return true;

        return a.AimAngle != b.AimAngle ||
            a.CoverToggle != b.CoverToggle ||
            a.Fire1 != b.Fire1 ||
            a.Fire2 != b.Fire2 ||
            a.Horizontal != b.Horizontal ||
            a.Vertical != b.Vertical ||
            a.IsAiming != b.IsAiming ||
            a.IsCrouching != b.IsCrouching ||
            a.IsSprinting != b.IsSprinting ||
            a.IsWalking != b.IsWalking ||
            a.Reload != b.Reload ||
            a.RotationAngleY != b.RotationAngleY ||
            a.AimTargetX != b.AimTargetX ||
            a.AimTargetY != b.AimTargetY ||
            a.AimTargetZ != b.AimTargetZ ||
            a.DamageTaken != b.DamageTaken;
    }

    private BinaryFormatter bf = new BinaryFormatter();

    private byte[] SerializeState(NetworkState state)
    {
        using(MemoryStream stream = new MemoryStream())
        {
            bf.Serialize(stream, state);
            return stream.ToArray();
        }
    }

    private NetworkState DeserializeState(byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            return (NetworkState) bf.Deserialize(stream);
        }
    }
}
