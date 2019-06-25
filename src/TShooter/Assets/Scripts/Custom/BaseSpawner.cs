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
        Transform baseSpawnPoint;
        List<BaseSpawnPoint> baseSpawnPoints = new List<BaseSpawnPoint>();
        baseSpawnPointContainer.GetComponentsInChildren<BaseSpawnPoint>(baseSpawnPoints);

        if (baseSpawnPointContainer != null && baseSpawnPoints.Count != 0)
        {
            baseSpawnPoints = baseSpawnPoints.Where((arg) => arg.GetComponents<Base>().Length == 0).ToList();
            int randomSpawnPointIndex = Random.Range(0, baseSpawnPoints.Count);
            baseSpawnPoint = baseSpawnPoints[randomSpawnPointIndex].transform;
        }
        else
        {
            baseSpawnPoint = transform;
        }
        return baseSpawnPoint;
    }
}
