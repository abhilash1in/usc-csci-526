using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(Collider))]
public class Destructable : CustomNetworkBehviour
{
    [SerializeField] float hitPoints;
    public event System.Action OnDeath;
    public event System.Action<float> OnDamageReceived;


    [SyncVar]
    [SerializeField]
    private float m_DamageTaken;


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

    private void setDamageTaken(float d)
    {
        print("Hook invoked");
        if (!isServer)
        {
            print("Hook invoked in client: " + d + ". Team : " + TeamID);
            m_DamageTaken = d;
        }
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

        if(HitPointsRemaining <= 0)
        {
            Die();
        }
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
