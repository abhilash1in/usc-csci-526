using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] Collider trigger;
    PlayerCover playerCover;

    bool IsLocalPlayer(Collider other)
    {
        if (other.tag != "Player")
            return false;

        // if we are not the local player
        if (other.GetComponent<Player>() != GameManager.Instance.LocalPLayer)
            return false;

        playerCover = GameManager.Instance.LocalPLayer.GetComponent<PlayerCover>();
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsLocalPlayer(other))
            return;

        playerCover.SetPlayerCoverAllowed(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsLocalPlayer(other))
            return;

        playerCover.SetPlayerCoverAllowed(false);
    } 
}
