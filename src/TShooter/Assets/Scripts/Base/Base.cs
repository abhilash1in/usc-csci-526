using UnityEngine;
using UnityEngine.Networking;

public class Base : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float respawnDuration = 2f;

    [SerializeField]
    private float remainingHitsToRespawn;

    [SerializeField]
    private float respawnThreshold = 10f;

    [SyncVar]
    [SerializeField]
    private float currentHealth;

    private static int counter = 1;
    public string teamID = "";
    
    void Awake()
    {
        setDefaults();
    }

    void Start()
    {
        teamID = (Base.counter).ToString();
        counter += 1;

        GameManager.RegisterBase(teamID, GetComponent<Base>());
    }

    float GetRemainingHitsToRespawn()
    {
        return maxHealth / respawnThreshold;
    }

    public void setDefaults()
    {
        currentHealth = maxHealth;
        remainingHitsToRespawn = GetRemainingHitsToRespawn();
    }

    [ClientRpc]
    public void RpcTakeBaseDamage(float _amount)
    {
        currentHealth -= _amount;
        remainingHitsToRespawn -= _amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else if(remainingHitsToRespawn <= 0)
        {
            remainingHitsToRespawn = GetRemainingHitsToRespawn();
            GameManager.Instance.Respawner.RespawnBase(gameObject, respawnDuration);
        }
        Debug.Log(transform.name + "now has " + currentHealth + " health");
    }
}
