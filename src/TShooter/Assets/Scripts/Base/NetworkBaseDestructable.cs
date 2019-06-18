using UnityEngine;
using UnityEngine.Networking;

public class NetworkBaseDestructable : NetworkBehaviour
{
    [Client]
    public void ShootBase(string name, float damage)
    {
        CmdBaseShot(name, damage);
    }

    [Command]
    void CmdBaseShot(string tID, float _damage)
    {
        Base b = GameManager.GetBase(tID);
        b.RpcTakeBaseDamage(_damage);
    }
}