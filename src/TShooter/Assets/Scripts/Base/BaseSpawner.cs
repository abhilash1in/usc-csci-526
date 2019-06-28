using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] GameObject baseSpawnPointContainer;

    public Transform GetNewSpawnPointTransform()
    {
        List<BaseSpawnPoint> baseSpawnPoints = new List<BaseSpawnPoint>();
        baseSpawnPointContainer.GetComponentsInChildren<BaseSpawnPoint>(baseSpawnPoints);
        baseSpawnPoints = baseSpawnPoints.Where((arg) => !arg.IsOccupied).ToList();
        return baseSpawnPoints[Random.Range(0, baseSpawnPoints.Count)].transform;
    }

    public Transform GetSpawnPointWithName(string spName)
    {
        return baseSpawnPointContainer.transform.Find(spName);
    }
}
