using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class MyNetworkDiscovery : NetworkDiscovery
{
    bool initialized;
    public LobbyManager lobbyManager;
    // Start is called before the first frame update
    void Start()
    {
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        GameManager.Instance.EventBus.AddListener("StartServerBroadcast", StartServerBroadcast);
        GameManager.Instance.EventBus.AddListener("StopServerBroadcast", StopServerBroadcast);
        GameManager.Instance.EventBus.AddListener("StartClientReceive", StartClientReceive);
        GameManager.Instance.EventBus.AddListener("StopClientReceive", StopClientReceive);
    }

    void StartServerBroadcast()
    {
        initialized = Initialize();
        if (!initialized)
        {
            HelperMethods.ShowMessage("Cannot initialize Network Discovery");
            return;
        }
        StartAsServer();
        Debug.Log("Started broadcasting.");
        GameManager.Instance.EventBus.RaiseEvent("StartedServerBroadcast");
    }

    void StopServerBroadcast()
    {
        if (initialized)
        {
            StopBroadcast();
            initialized = false;
            Debug.Log("Stopped broadcasting.");
            GameManager.Instance.EventBus.RaiseEvent("StoppedServerBroadcast");
        }
    }

    void StartClientReceive()
    {
        initialized = Initialize();
        if (!initialized)
        {
            HelperMethods.ShowMessage("Cannot initialize Network Discovery");
            return;
        }

        StartAsClient();
        Debug.Log("Started listening.");
        GameManager.Instance.EventBus.RaiseEvent("StartedClientReceive");
    }

    void StopClientReceive()
    {
        if (initialized)
        {
            StopBroadcast();
            initialized = false;
            Debug.Log("Stopped listening.");
            GameManager.Instance.EventBus.RaiseEvent("StoppedClientReceive");
        }
        else
        {
            Debug.Log("StopClientReceive Not Initialized.");
        }
    }


    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Got broadcast from [" + fromAddress + "] " + data);
        StopClientReceive();
        lobbyManager.loadingPanel.ToggleVisibility(false);
        lobbyManager.ChangeTo(lobbyManager.lobbyPanel);

        lobbyManager.networkAddress = fromAddress;
        lobbyManager.StartClient();

        lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        //lobbyManager.DisplayIsConnecting();

        lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
    }
}
