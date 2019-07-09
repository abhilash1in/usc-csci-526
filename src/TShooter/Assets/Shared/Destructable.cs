using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(Collider))]
public class Destructable : CustomNetworkBehviour
{
    [SerializeField] float hitPoints;

    [SerializeField] bool autoHealthBoost;
    [SerializeField] int boostDuration;

    public event System.Action OnDeath;
    public event System.Action<Destructable> OnAdvancedDeath;
    public event System.Action<float> OnDamageReceived;


    [SyncVar(hook = "setDamageTaken")]
    [SerializeField]
    private float m_DamageTaken;
    
    private void Start()
    {
        if (autoHealthBoost)
        {
            print("autoHealthBoost enabled");
            if(isServer)
                BoostHealth();
        }
        else
        {
            print("autoHealthBoost DISABLED");
        }
    }

    void BoostHealth()
    {
        GameManager.Instance.Timer.Add(() => {
            if (m_DamageTaken > 0 && !GameManager.Instance.IsPaused)
            {
                print("boosting health");
                m_DamageTaken = m_DamageTaken - 1;
                UpdateHealthBar();
            }
            BoostHealth();
        }, boostDuration);
    }


    public float DamageTaken
    {
        get
        {
            return m_DamageTaken;
        }

        set
        {
            m_DamageTaken = value;
            if (HitPointsRemaining <= 0)
            {
                Die();
            }
        }
    }

    private void UpdateHealthBar()
    {
        float damage = HitPointsRemaining / hitPoints;
        GameObject.Find("UICanvas").GetComponent<HealthBar>().SetHealthAmount(TeamID, damage);
    }

    private void setDamageTaken(float d)
    {
        print("Hook invoked : " + d);
        if (!isServer)
        {
            print("Hook invoked in client: " + d + ". Team : " + TeamID);
            m_DamageTaken = d;
        }
        UpdateHealthBar();
    }


    public float HitPointsRemaining
    {
        get
        {
            return hitPoints - m_DamageTaken;
        }
        set
        {
            HitPointsRemaining = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return HitPointsRemaining > 0;
        }
    }

    public virtual void Die()
    {
        if (IsAlive)
        {
            return;
        }

        if(OnDeath != null)
        {
            OnDeath();
        }

        if (OnAdvancedDeath != null)
        {
            OnAdvancedDeath(this);
        }
    }

    private void Update()
    {
        if (HitPointsRemaining <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        print("TakeDamage: " + amount);
        if (!IsAlive)
            return;

        if (!isServer)
            return;

        m_DamageTaken += amount;

        if(OnDamageReceived != null)
        {
            OnDamageReceived(amount);
        }

        if (HitPointsRemaining <= 0)
        {
            Die();
        }

        Base b = GetComponent<Base>();
        if (b == null)
            return;

        float damage = HitPointsRemaining / hitPoints;
        GameObject.Find("UICanvas").GetComponent<HealthBar>().SetHealthAmount(TeamID, damage);
    }

    public void AssignTeam(CustomNetworkBehviour.ETeamID id)
    {
        this.TeamID = id;
    }

    public void Reset()
    {
        m_DamageTaken = 0;
    }
}
