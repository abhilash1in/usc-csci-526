using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class Base : CustomNetworkBehviour
{
    [SyncVar(hook = "UpdateSpawnPoint")] public string spawnPointName;

    [SerializeField]
    Material redMaterial;

    [SerializeField]
    Material blueMaterial;
    
    Renderer renderer;

    private BaseSpawner m_Spawner;
    public BaseSpawner BaseSpawner
    {
        get
        {
            if (m_Spawner == null)
                m_Spawner = GetComponent<BaseSpawner>();
            return m_Spawner;
        }
    }

    private void Start()
    {
        UpdateMaterial();
        if (isServer)
        {
            SetSpawnPoint(BaseSpawner.GetNewSpawnPointTransform().name);
        }
    }

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        //UpdateMaterial();
    }

    public void SetSpawnPoint(string spName)
    {
        spawnPointName = spName;
        UpdateSpawnPoint(spName);
    }

    public void UpdateSpawnPoint(string spName)
    {
        Transform spawnPointTransform = BaseSpawner.GetSpawnPointWithName(spName);
        if(spawnPointTransform != null)
        {
            transform.SetParent(spawnPointTransform);
            transform.position = spawnPointTransform.position;
            transform.rotation = spawnPointTransform.rotation;
        }
    }


    public void UpdateMaterial()
    {
        if(renderer != null)
        {
           if(TeamID == CustomNetworkBehviour.ETeamID.RED)
            {
                renderer.material = redMaterial;
            }

            if (TeamID == CustomNetworkBehviour.ETeamID.BLUE)
            {
                renderer.material = blueMaterial;
            }
        }
    }
}
