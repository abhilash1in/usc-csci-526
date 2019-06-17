using UnityEngine;
using UnityEngine.Networking;

public class SpawnBases : NetworkBehaviour
{

    public GameObject basePrefab;
    // public Transform spawnPoint01;
    // public Transform spawnPoint02;
    public int randomizeWaitThreshold = 5;
    [SerializeField] GameObject BaseSpawnPointContainer;

    void Start()
    {
        CmdSpawnBases();
    }


    [Command]
    public void CmdSpawnBases()
    {
        int spawnIndex1, spawnIndex2;
        int randomizeIndex = 0;


        SpawnPoint[] baseSpawnPoints = BaseSpawnPointContainer.GetComponentsInChildren<SpawnPoint>();

        spawnIndex1 = Random.Range(0, baseSpawnPoints.Length);
        spawnIndex2 = Random.Range(0, baseSpawnPoints.Length);

        while(spawnIndex1 == spawnIndex2)
        {
            if (randomizeIndex > randomizeWaitThreshold)
                break;
            spawnIndex2 = Random.Range(0, baseSpawnPoints.Length);
            randomizeIndex++;
        }

        GameObject b1 = Instantiate(basePrefab, baseSpawnPoints[spawnIndex1].transform);
        GameObject b2 = Instantiate(basePrefab, baseSpawnPoints[spawnIndex2].transform);

        NetworkServer.Spawn(b1);
        NetworkServer.Spawn(b2);
    }
}
