using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
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

    // Use this for initialization
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Vertical", GameManager.Instance.InputController.Vertical);
        animator.SetFloat("Horizontal", GameManager.Instance.InputController.Horizontal);
        animator.SetBool("IsWalking", GameManager.Instance.InputController.IsWalking);
        animator.SetBool("IsSprinting", GameManager.Instance.InputController.IsSprinting);
        animator.SetBool("IsCrouching", GameManager.Instance.InputController.IsCrouching);
        animator.SetFloat("AimAngle", PlayerAim.GetAngle());
        animator.SetBool("IsAiming",
            GameManager.Instance.LocalPLayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING ||
            GameManager.Instance.LocalPLayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING
        );
    }
}
