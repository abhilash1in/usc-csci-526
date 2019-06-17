using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] Vector2 xLimits = new Vector2(-50, 50);
    [SerializeField] Vector2 zLimits = new Vector2(-50, 50);
    GameObject BaseSpawnPointContainer;
    SpawnPoint[] baseSpawnPoints;

    private void Start()
    {
        BaseSpawnPointContainer = GameObject.Find("BaseSpawnPointContainer");
        if(BaseSpawnPointContainer != null)
        {
            baseSpawnPoints = BaseSpawnPointContainer.GetComponentsInChildren<SpawnPoint>();
        }
    }

    public void Despawn(GameObject go, float inSeconds)
    {
        go.SetActive(false);
        GameManager.Instance.Timer.Add(() =>
        {
            go.SetActive(true);
        }, inSeconds);
    }

    public void DespawnRandom(GameObject go, float inSeconds)
    {
        go.SetActive(false);
        GameManager.Instance.Timer.Add(() =>
        {
            go.transform.position = new Vector3(Random.Range(xLimits.x, xLimits.y), go.transform.position.y, Random.Range(zLimits.x, zLimits.y));
            go.SetActive(true);
        }, inSeconds);
    }

    public void RespawnBase(GameObject go, float inSeconds)
    {
        go.SetActive(false);

        if (baseSpawnPoints.Length == 0)
        {
            GameManager.Instance.Timer.Add(() =>
            {
                go.transform.position = new Vector3(Random.Range(xLimits.x, xLimits.y), go.transform.position.y, Random.Range(zLimits.x, zLimits.y));
                go.SetActive(true);
            }, inSeconds);
        }
        else
        {
            GameManager.Instance.Timer.Add(() =>
            {
                int index = Random.Range(0, baseSpawnPoints.Length);
                go.transform.SetParent(baseSpawnPoints[index].transform);
                go.transform.position = baseSpawnPoints[index].transform.position;
                go.transform.rotation = baseSpawnPoints[index].transform.rotation;
                go.SetActive(true);
            }, inSeconds);

        }
    }
}
