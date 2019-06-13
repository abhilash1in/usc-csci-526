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
    [SerializeField] Transform aimTarget;

    public WeaponReloader Reloader;
    private ParticleSystem muzzleFireParticleSystem;



    float nextFireAllowed;
    Transform muzzle;

    public bool canFire;

    public void Equip() 
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Awake()
    {
        muzzle = transform.Find("Model/Muzzle");
        Reloader = GetComponent<WeaponReloader>();
        muzzleFireParticleSystem = muzzle.GetComponent<ParticleSystem>();
    }

    public void Reload()
    {
        if (Reloader == null)
            return;

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

        if (Reloader != null)
        {
            if (Reloader.IsReloading)
                return;
            if (Reloader.RoundsRemainingInClip == 0)
                return;

            Reloader.TakeFromClip(1);
        }
        nextFireAllowed = Time.time + rateOfFire;

        muzzle.LookAt(aimTarget);
        FireEffect();

        // instantiate the projectile
        Instantiate(projectile, muzzle.position, muzzle.rotation);
        audioFire.Play();
        canFire = true;

    }
}
