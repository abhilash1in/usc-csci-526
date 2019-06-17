using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{   
    enum ESpawnPointType
    {
        PLAYER,
        BASE
    }

    [SerializeField] ESpawnPointType SpawnPointType = ESpawnPointType.PLAYER;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;

        if(SpawnPointType == ESpawnPointType.BASE)
        {
            Gizmos.DrawWireSphere(Vector3.zero + Vector3.up * 1, 5f);
        }
        else
        {
            Gizmos.DrawWireCube(Vector3.zero + Vector3.up * 1, Vector3.one + Vector3.up * 1);
        }
    }
}
