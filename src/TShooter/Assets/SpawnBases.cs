using UnityEngine;
using UnityEngine.Networking;

public class SpawnBases : NetworkBehaviour
{

    public GameObject basePrefab1;
    public GameObject basePrefab2;
    // public Transform spawnPoint01;
    // public Transform spawnPoint02;
    public int randomizeWaitThreshold = 5;
    GameObject BaseSpawnPointContainer;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if(isServer && isLocalPlayer)
        {
            CmdSpawnBases();
        }
    }

    [Command]
    public void CmdSpawnBases()
    {
        int spawnIndex1, spawnIndex2;
        int randomizeIndex = 0;

        BaseSpawnPointContainer = GameObject.Find("BaseSpawnPointContainer");
        SpawnPoint[] baseSpawnPoints = BaseSpawnPointContainer.GetComponentsInChildren<SpawnPoint>();

        spawnIndex1 = Random.Range(0, baseSpawnPoints.Length);
        spawnIndex2 = Random.Range(0, baseSpawnPoints.Length);

        while (spawnIndex1 == spawnIndex2)
        {
            if (randomizeIndex > randomizeWaitThreshold)
                break;
            spawnIndex2 = Random.Range(0, baseSpawnPoints.Length);
            randomizeIndex++;
        }

        GameObject b1 = Instantiate(basePrefab1, baseSpawnPoints[spawnIndex1].transform);

        GameObject b2 = Instantiate(basePrefab2, baseSpawnPoints[spawnIndex2].transform);

        NetworkServer.Spawn(b1);
        NetworkServer.Spawn(b2);

    }
}
