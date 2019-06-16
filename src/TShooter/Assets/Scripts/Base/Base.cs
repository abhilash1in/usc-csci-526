using UnityEngine;
using UnityEngine.Networking;

public class Base : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SyncVar]
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

    public void setDefaults()
    {
        currentHealth = maxHealth;
    }

    [ClientRpc]
    public void RpcTakeBaseDamage(float _amount)
    {
        currentHealth -= _amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(transform.name + "now has" + currentHealth + "health");
    }
}
