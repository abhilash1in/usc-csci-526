using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] Vector2 xLimits = new Vector2(-50, 50);
    [SerializeField] Vector2 zLimits = new Vector2(-50, 50);

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
}
