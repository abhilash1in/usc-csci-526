using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamInfo : MonoBehaviour
{
    [SerializeField] Text text;
    PlayerNetwork playerNetwork;
    int originalFontSize;

    void Start()
    {
        originalFontSize = text.fontSize;
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
    }

    void Instance_OnLocalPlayerJoined(Player player)
    {
        playerNetwork = player.PlayerNetwork;
    }

    private void Update()
    {
        if (playerNetwork == null)
            return;

        if (playerNetwork.isFrozen)
        {
            text.fontSize = originalFontSize / 2;
            text.text = "FROZEN";
        }
        else
        {
            text.fontSize = originalFontSize;
            switch (playerNetwork.TeamID)
            {
                case CustomNetworkBehviour.ETeamID.RED:
                    text.color = new Color(209, 86, 48);
                    break;
                case CustomNetworkBehviour.ETeamID.BLUE:
                    text.color = new Color(97, 105, 184);
                    break;
                default:
                    text.color = Color.white;
                    break;
            }
            text.text = playerNetwork.TeamID.ToString();
        }
    }

}
