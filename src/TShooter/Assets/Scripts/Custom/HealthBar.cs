using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthBar : CustomNetworkBehviour
{
    [SerializeField]
    RectTransform healthAmountRed;

    [SerializeField]
    RectTransform healthAmountBlue;

    public void SetHealthAmount(ETeamID teamID, float _amount)
    {
        switch(teamID)
        {
            case ETeamID.RED:
                healthAmountRed.localScale = new Vector3(1f, _amount, 1f);
                break;
            case ETeamID.BLUE:
                healthAmountBlue.localScale = new Vector3(1f, _amount, 1f);
                break;
            default:
                break;
        }
         
    }
}
