using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CustomNetworkBehviour : NetworkBehaviour
{
    public enum ETeamID
    {
        NONE,
        BLUE,
        RED
    }

    [SyncVar] public ETeamID TeamID;

    public void AssignTeam(CustomNetworkBehviour.ETeamID id)
    {
        this.TeamID = id;
    }
}
