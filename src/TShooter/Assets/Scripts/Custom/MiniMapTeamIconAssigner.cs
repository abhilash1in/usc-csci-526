using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base))]
[RequireComponent(typeof(MiniMapComponent))]
public class MiniMapTeamIconAssigner : MonoBehaviour
{
    [SerializeField] Sprite redBaseSprite;
    [SerializeField] Sprite blueBaseSprite;

    Base baseGO;
    MiniMapComponent miniMapComponent;
    bool teamIconAssigned;

    private void Awake()
    {
        baseGO = GetComponent<Base>();
        miniMapComponent = GetComponent<MiniMapComponent>();
        assignIcon();
    }

    void assignIcon()
    {
        if (baseGO.TeamID == CustomNetworkBehviour.ETeamID.NONE)
        {
            teamIconAssigned = false;
        }
        else if (baseGO.TeamID == CustomNetworkBehviour.ETeamID.RED)
        {
            teamIconAssigned = true;
            miniMapComponent.icon = redBaseSprite;
        }
        else if (baseGO.TeamID == CustomNetworkBehviour.ETeamID.BLUE)
        {
            teamIconAssigned = true;
            miniMapComponent.icon = blueBaseSprite;
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
