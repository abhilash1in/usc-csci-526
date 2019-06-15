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
    public bool isAiming;

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

    private Player m_player;

    public Player Player
    {
        get
        {
            if(m_player == null)
            {
                m_player = GetComponent<Player>();
            }
            return m_player;
        }
    }

    void GetLocalPlayerInput()
    {
        Vertical = Player.InputState.Vertical;
        Horizontal = Player.InputState.Horizontal;
        IsWalking = Player.InputState.IsWalking;
        IsSprinting = Player.InputState.IsSprinting;
        IsCrouching = Player.InputState.IsCrouching;


        AimAngle = PlayerAim.GetAngle();
        isAiming = GameManager.Instance.LocalPLayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING ||
            GameManager.Instance.LocalPLayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING;
    }

    // Use this for initialization
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsLocalPlayer)
            GetLocalPlayerInput();

        animator.SetFloat("Vertical", Vertical);
        animator.SetFloat("Horizontal", Horizontal);
        animator.SetBool("IsWalking", IsWalking);
        animator.SetBool("IsSprinting", IsSprinting);
        animator.SetBool("IsCrouching", IsCrouching);
        animator.SetFloat("AimAngle", AimAngle);
        animator.SetBool("IsAiming", isAiming);
    }
}
