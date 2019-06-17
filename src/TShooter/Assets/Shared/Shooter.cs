using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float rateOfFire;
    [SerializeField] Projectile projectile;
    [SerializeField] Transform hand;
    [SerializeField] AudioController audioReload;
    [SerializeField] AudioController audioFire;
    [SerializeField] Vector3 aimPoint;

    // TODO - temporary workaround
    public Transform AimTarget;
    public Vector3 AimTargetOffset;

    public WeaponReloader Reloader;
    private ParticleSystem muzzleFireParticleSystem;

    Player player;

    float nextFireAllowed;
    Transform muzzle;

    public bool canFire;

    public void Equip() 
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void SetAimPoint(Vector3 target)
    {
        aimPoint = target;
    }

    public Vector3 GetImapctPoint()
    {
        // TODO - temporary fix
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        //RaycastHit hit;

        //Vector3 targetPosition = ray.GetPoint(500);

        //if (Physics.Raycast(ray, out hit))
        //{
        //    return hit.point;
        //}


        //return transform.position + transform.forward * 50;

        return AimTarget.position + AimTargetOffset;

    }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        muzzle = transform.Find("Model/Muzzle");
        Reloader = GetComponent<WeaponReloader>();
        muzzleFireParticleSystem = muzzle.GetComponent<ParticleSystem>();
    }

    public void Reload()
    {
        if (Reloader == null)
            return;

        if (player.IsLocalPlayer)
            Reloader.Reload();

        audioReload.Play();
    }

    void FireEffect()
    {
        if (muzzleFireParticleSystem == null)
            return;
        muzzleFireParticleSystem.Play();
    }

    public virtual void Fire() 
    {
        canFire = false;

        if (Time.time < nextFireAllowed)
            return;

        if(player.IsLocalPlayer && Reloader != null)
        {
            if (Reloader.IsReloading)
                return;
            if (Reloader.RoundsRemainingInClip == 0)
                return;

            Reloader.TakeFromClip(1);
        }

        nextFireAllowed = Time.time + rateOfFire;

        //muzzle.LookAt(aimPoint);
        muzzle.LookAt(AimTarget.position + AimTargetOffset);
        FireEffect();

        // instantiate the projectile
        Instantiate(projectile, muzzle.position, muzzle.rotation);
        audioFire.Play();
        canFire = true;
    }
}
