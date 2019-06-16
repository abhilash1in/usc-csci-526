using UnityEngine;
using UnityEngine.Networking;

public class SpawnBases : NetworkBehaviour
{

    public GameObject basePrefab;
    public Transform spawnPoint01;
    public Transform spawnPoint02;

    void Start()
    {
        CmdSpawnBases();
    }

    [Command]
    public void CmdSpawnBases()
    {
        GameObject b1 = Instantiate(basePrefab, spawnPoint01);
        GameObject b2 = Instantiate(basePrefab, spawnPoint02);
        
        NetworkServer.Spawn(b1);
        NetworkServer.Spawn(b2);
    }
}
