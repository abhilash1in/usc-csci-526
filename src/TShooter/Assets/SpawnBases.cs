using UnityEngine;
using UnityEngine.Networking;

public class SpawnBases : NetworkBehaviour
{

    public GameObject basePrefab;
    // public Transform spawnPoint01;
    // public Transform spawnPoint02;
    public int randomizeWaitThreshold = 5;
    [SerializeField] GameObject BaseSpawnPointContainer;

    public override void OnStartServer()
    {
        SpawnGameBases();
    }

    public void SpawnGameBases()
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

        GameObject b1 = Instantiate(basePrefab, baseSpawnPoints[0].transform);
        b1.GetComponent<Renderer>().material.color = Color.blue;

        GameObject b2 = Instantiate(basePrefab, baseSpawnPoints[1].transform);
        b2.GetComponent<Renderer>().material.color = Color.green;

        NetworkServer.Spawn(b1);
        NetworkServer.Spawn(b2);
    }
}
