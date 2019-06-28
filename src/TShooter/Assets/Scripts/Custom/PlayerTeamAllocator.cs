using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerTeamAllocator : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);

        Player player = gamePlayer.GetComponent<Player>();
        PlayerNetwork playerNetworkInstance = gamePlayer.GetComponent<PlayerNetwork>();
        LobbyPlayer lobbyPlayerInstance = lobbyPlayer.GetComponent<LobbyPlayer>();

        if (player == null || playerNetworkInstance == null || lobbyPlayerInstance == null)
            return;

        if(lobbyPlayerInstance.playerColor == Color.red)
        {
            player.name = "RedPlayer" + lobbyPlayerInstance.playerName;
            playerNetworkInstance.AssignTeam(CustomNetworkBehviour.ETeamID.RED);
        }
        else
        {
            player.name = "BluePlayer" + lobbyPlayerInstance.playerName;
            playerNetworkInstance.AssignTeam(CustomNetworkBehviour.ETeamID.BLUE);
        }
    }
}
