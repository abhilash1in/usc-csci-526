using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    public float Vertical;
    public float Horizontal;
    public bool IsWalking;
    public bool IsSprinting;
    public bool IsCrouching;
    public float AimAngle;
    public bool IsAiming;
    public bool IsInCover;


    private PlayerAim m_PlayerAim;
    private PlayerAim PlayerAim
    {
        get
        {
            if (m_PlayerAim == null)
                m_PlayerAim = GameManager.Instance.LocalPLayer.playerAim;
            return m_PlayerAim;
        }
    }

    private Player m_Player;
    private Player Player
    {
        get
        {
            if (m_Player == null)
                m_Player = GetComponent<Player>();
            return m_Player;
        }

    }

    void GetLocalPlayerInput()
    {
        Vertical = Player.InputState.Vertical;
        Horizontal = Player.InputState.Horizontal;

        IsWalking = GameManager.Instance.LocalPLayer.PlayerState.MoveState == PlayerState.EMoveState.WALKING;
        IsSprinting = Player.InputState.IsSprinting;
        IsCrouching = Player.InputState.IsCrouching;

        AimAngle = PlayerAim.GetAngle();
        IsAiming = GameManager.Instance.LocalPLayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING ||
            GameManager.Instance.LocalPLayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING;

        IsInCover = GameManager.Instance.LocalPLayer.PlayerState.MoveState == PlayerState.EMoveState.COVER;
    }

    // Use this for initialization
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        if (Player.IsLocalPlayer)
            GetLocalPlayerInput();

        if(Player.PlayerNetwork != null && Player.PlayerNetwork.isFrozen)
        {
            Vertical = 0;
            Horizontal = 0;
        }


        animator.SetFloat("Vertical", Vertical);
        animator.SetFloat("Horizontal", Horizontal);
        animator.SetBool("IsWalking", IsWalking);
        animator.SetBool("IsSprinting", IsSprinting);
        animator.SetBool("IsCrouching", IsCrouching);
        animator.SetFloat("AimAngle", AimAngle);
        animator.SetBool("IsAiming", IsAiming);
        animator.SetBool("IsInCover", IsInCover);
    }
}
