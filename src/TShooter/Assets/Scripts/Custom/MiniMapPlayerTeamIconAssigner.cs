using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(MiniMapComponent))]
public class MiniMapPlayerTeamIconAssigner : MonoBehaviour
{
    [SerializeField] Sprite redTeamSprite;
    [SerializeField] Sprite blueTeamSprite;
    [SerializeField] Sprite unknownSprite;

    Player playerGO;
    MiniMapComponent miniMapComponent;
    bool teamIconAssigned;

    void Awake()
    {
        playerGO = GetComponent<Player>();
        miniMapComponent = GetComponent<MiniMapComponent>();
        assignIcon();
    }

    void assignIcon()
    {
        if (playerGO.PlayerNetwork.TeamID == CustomNetworkBehviour.ETeamID.NONE)
        {
            teamIconAssigned = false;
        }
        else if (playerGO.PlayerNetwork.TeamID == CustomNetworkBehviour.ETeamID.RED)
        {
            teamIconAssigned = true;
            miniMapComponent.icon = redTeamSprite;
            miniMapComponent.enabled = false;
            miniMapComponent.enabled = true;
        }
        else if (playerGO.PlayerNetwork.TeamID == CustomNetworkBehviour.ETeamID.BLUE)
        {
            teamIconAssigned = true;
            miniMapComponent.icon = blueTeamSprite;
            miniMapComponent.enabled = false;
            miniMapComponent.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!teamIconAssigned)
        {
            assignIcon();
        }
    }
}
