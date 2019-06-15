using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    CharacterController characterController;

    //[SerializeField]
    //GameObject playerUIPrefab;
    //private GameObject playerUIInstance;

    //Camera sceneCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
                characterController.enabled = false;
            }

            //gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
        }
        else
        {
            //sceneCamera = Camera.main;
            //if (sceneCamera != null)
            //{
            //    sceneCamera.gameObject.SetActive(false);
            //}

            //GetComponent<Player>().PlayerSetup();

            //playerUIInstance = Instantiate(playerUIPrefab);
            //playerUIInstance.name = playerUIPrefab.name;
        }
    }

    //public override void OnStartLocalPlayer()
    //{
    //    base.OnStartLocalPlayer();

    //    GameManager.localPlayerName = "Player " + GetComponent<NetworkIdentity>().netId.ToString();
    //}

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //    GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<Player>());
    //    //GetComponent<Spawner>().CmdSpawnBases();
    //}

    //void OnDisable()
    //{

    //    //Destroy(playerUIInstance);

    //    if (sceneCamera != null)
    //    {
    //        sceneCamera.gameObject.SetActive(true);
    //    }

    //    GameManager.UnregisterPlayer(transform.name);
    //}
}
