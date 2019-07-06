using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class CustomNetworkManager : NetworkManager
{
    // Server callbacks
    int clientCount = -1;

    [SerializeField] GameObject basePrefab;
    [SerializeField] Material blueMaterial;
    [SerializeField] Material redMaterial;

    [SerializeField] float RandomRange;

    [SerializeField] bool IsTeamRedBaseInitialized;
    [SerializeField] bool IsTeamBlueBaseInitialized;

    public override void OnServerConnect(NetworkConnection conn)
    {

        Debug.Log("A client connected to the server: " + conn);

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //clientCount--;
        NetworkServer.DestroyPlayersForConnection(conn);

        if (conn.lastError != NetworkError.Ok)
        {

            if (LogFilter.logError) { Debug.LogError("ServerDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("A client disconnected from the server: " + conn);

    }

    public override void OnServerReady(NetworkConnection conn)
    {

        NetworkServer.SetClientReady(conn);

        Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);

    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //base.OnServerAddPlayer(conn, playerControllerId);
        clientCount++;
        Transform spawn = GetStartPosition();
        var player = (GameObject) GameObject.Instantiate(playerPrefab, spawn.position, Quaternion.identity);
        if (clientCount % 2 == 0)
        {
            print("TEAM BLUE");
            player.name = "BluePlayer" + clientCount;
            player.GetComponent<PlayerNetwork>().SetPlayerTeam(CustomNetworkBehviour.ETeamID.BLUE);
        }
        else
        {
            player.name = "RedPlayer" + clientCount;
            player.GetComponent<PlayerNetwork>().SetPlayerTeam(CustomNetworkBehviour.ETeamID.RED);
        }
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public void OnServerAddPlayerOld(NetworkConnection conn, short playerControllerId)
    {
        clientCount++;

        // Spawn base
        Transform baseSpawnPoint;
        GameObject baseSpawnPointContainer = GameObject.Find("BaseSpawnPointContainer");
        List<BaseSpawnPoint> baseSpawnPoints = new List<BaseSpawnPoint>();
        baseSpawnPointContainer.GetComponentsInChildren<BaseSpawnPoint>(baseSpawnPoints);

        if (baseSpawnPointContainer != null && baseSpawnPoints.Count != 0)
        {
            baseSpawnPoints = baseSpawnPoints.Where((arg) => !arg.IsOccupied).ToList();
            int randomSpawnPointIndex = Random.Range(0, baseSpawnPoints.Count);
            baseSpawnPoint = baseSpawnPoints[randomSpawnPointIndex].transform;
        }
        else
        {
            baseSpawnPoint = transform;
        }

        if (clientCount == 0)
        {
            // add BLUE base
            GameObject blueBaseGO = GameObject.Instantiate(basePrefab, baseSpawnPoint);
            Base blueBase = blueBaseGO.GetComponent<Base>();
            BaseHealth blueBaseHealth = blueBaseGO.GetComponent<BaseHealth>();
            blueBase?.SetSpawnPoint(baseSpawnPoint.name);
            blueBase?.AssignTeam(CustomNetworkBehviour.ETeamID.BLUE);
            blueBaseHealth?.AssignTeam(CustomNetworkBehviour.ETeamID.BLUE);
            blueBaseGO.name = "BlueBase";
            NetworkServer.Spawn(blueBaseGO);
        } 
        else if(clientCount == 1)
        {
            // add RED base
            GameObject redBaseGO = GameObject.Instantiate(basePrefab, baseSpawnPoint);
            Base redBase = redBaseGO.GetComponent<Base>();
            BaseHealth redBaseHealth = redBaseGO.GetComponent<BaseHealth>();
            redBase?.SetSpawnPoint(baseSpawnPoint.name);
            redBase?.AssignTeam(CustomNetworkBehviour.ETeamID.RED);
            redBaseHealth?.AssignTeam(CustomNetworkBehviour.ETeamID.RED);
            redBaseGO.name = "RedBase";
            NetworkServer.Spawn(redBaseGO);
        }

        // Spawn player
        var player = (GameObject) GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Vector3 playerPosition = new Vector3(Random.Range(0, RandomRange), 0, Random.Range(0, RandomRange));
        Quaternion playerRotation = Quaternion.identity;

        GameObject playerSpawnContainer = GameObject.Find("PlayerSpawnPointContainer");

        List<PlayerSpawnPoint> playerSpawnPoints = new List<PlayerSpawnPoint>();
        playerSpawnContainer.GetComponentsInChildren<PlayerSpawnPoint>(playerSpawnPoints);

        if(playerSpawnContainer != null && playerSpawnPoints.Count != 0)
        {
            playerSpawnPoints = playerSpawnPoints.Where((arg) => !arg.IsOccupied).ToList();
            int randomSpawnPointIndex = Random.Range(0, playerSpawnPoints.Count);
            playerPosition = playerSpawnPoints[randomSpawnPointIndex].transform.position;
            playerRotation = playerSpawnPoints[randomSpawnPointIndex].transform.rotation;
            playerSpawnPoints[randomSpawnPointIndex].IsOccupied = true;
        }

        player.transform.position = playerPosition;
        player.transform.rotation = playerRotation;

        if (clientCount % 2 == 0)
        {
            print("TEAM BLUE");
            player.name = "BluePlayer"+clientCount;
            player.GetComponent<PlayerNetwork>().SetPlayerTeam(CustomNetworkBehviour.ETeamID.BLUE);
        }
        else
        {
            player.name = "RedPlayer" + clientCount;
            player.GetComponent<PlayerNetwork>().SetPlayerTeam(CustomNetworkBehviour.ETeamID.RED);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        Debug.Log("Client has requested to get his player added to the game");

    }


    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {

        if (player.gameObject != null)

            NetworkServer.Destroy(player.gameObject);

    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {

        Debug.Log("Server network error occurred: " + (NetworkError)errorCode);

    }

    public override void OnStartHost()
    {

        Debug.Log("Host has started");
        //NetworkServer.Spawn()

    }

    public override void OnStartServer()
    {

        Debug.Log("Server has started");
    }

    public override void OnStopServer()
    {

        Debug.Log("Server has stopped");

    }

    public override void OnStopHost()
    {

        Debug.Log("Host has stopped");

    }

    // Client callbacks

    public override void OnClientConnect(NetworkConnection conn)

    {

        base.OnClientConnect(conn);

        Debug.Log("Connected successfully to server, now to set up other stuff for the client...");

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //clientCount--;
        StopClient();

        if (conn.lastError != NetworkError.Ok)

        {

            if (LogFilter.logError) { Debug.LogError("ClientDisconnected due to error: " + conn.lastError); }

        }

        Debug.Log("Client disconnected from server: " + conn);

    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {

        Debug.Log("Client network error occurred: " + (NetworkError)errorCode);

    }

    public override void OnClientNotReady(NetworkConnection conn)
    {

        Debug.Log("Server has set client to be not-ready (stop getting state updates)");

    }

    public override void OnStartClient(NetworkClient client)
    {

        Debug.Log("Client has started");

    }

    public override void OnStopClient()
    {
        clientCount--;
        Debug.Log("Client has stopped");

    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {

        base.OnClientSceneChanged(conn);

        Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");

    }

}
