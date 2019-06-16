using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerDestructable : NetworkBehaviour
{

    private void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<Player>().PlayerSetup();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        //GetComponent<NetworkIdentity>().netId.ToString()
        GameManager.RegisterPlayer((++Player.counter).ToString(), GetComponent<Player>());
    }

    [Client]
    public void Shoot(string name, float damage)
    {
        CmdPlayerShot(name, damage);
    }

    [Command]
    private void CmdPlayerShot(string playerID, float damage)
    {
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(damage);
    }
}

