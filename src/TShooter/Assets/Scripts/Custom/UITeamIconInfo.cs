using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITeamIconInfo : MonoBehaviour
{
    PlayerNetwork playerNetwork;

    [SerializeField] Transform redTeamIcon;
    [SerializeField] Transform blueTeamIcon;

    bool teamIconAssigned;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.LocalPLayer != null && GameManager.Instance.LocalPLayer.PlayerNetwork != null)
        {
            switch (GameManager.Instance.LocalPLayer.PlayerNetwork.TeamID)
            {
                case CustomNetworkBehviour.ETeamID.RED:
                    redTeamIcon.gameObject.SetActive(true);
                    blueTeamIcon.gameObject.SetActive(false);
                    break;
                case CustomNetworkBehviour.ETeamID.BLUE:
                    blueTeamIcon.gameObject.SetActive(true);
                    redTeamIcon.gameObject.SetActive(false);
                    break;
                default:
                    print("default");
                    break;
            }
            teamIconAssigned = true;
        }
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
    }

    private void Awake()
    {
        print("awake");
    }

    void Instance_OnLocalPlayerJoined(Player player)
    {
        print("Instance_OnLocalPlayerJoined");
        playerNetwork = player.PlayerNetwork;
    }

    // Update is called once per frame
    void Update()
    {
        if (teamIconAssigned)
            return;

        print(GameManager.Instance.LocalPLayer.PlayerNetwork.TeamID);
        if (playerNetwork != null)
        {
            switch (playerNetwork.TeamID)
            {
                case CustomNetworkBehviour.ETeamID.RED:
                    redTeamIcon.gameObject.SetActive(true);
                    blueTeamIcon.gameObject.SetActive(false);
                    break;
                case CustomNetworkBehviour.ETeamID.BLUE:
                    blueTeamIcon.gameObject.SetActive(true);
                    redTeamIcon.gameObject.SetActive(false);
                    break;
                default:
                    print("default");
                    break;
            }
            teamIconAssigned = true;
        }

        if(GameManager.Instance.LocalPLayer != null && GameManager.Instance.LocalPLayer.PlayerNetwork != null)
        {
            switch (GameManager.Instance.LocalPLayer.PlayerNetwork.TeamID)
            {
                case CustomNetworkBehviour.ETeamID.RED:
                    redTeamIcon.gameObject.SetActive(true);
                    blueTeamIcon.gameObject.SetActive(false);
                    break;
                case CustomNetworkBehviour.ETeamID.BLUE:
                    blueTeamIcon.gameObject.SetActive(true);
                    redTeamIcon.gameObject.SetActive(false);
                    break;
                default:
                    print("default");
                    break;
            }
            teamIconAssigned = true;
        }
    }
}
