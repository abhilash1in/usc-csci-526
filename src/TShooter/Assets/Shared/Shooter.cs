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
    [SerializeField] bool shouldRecoil = true;

    public Transform AimTarget;
    public Vector3 AimTargetOffset;
    public WeaponReloader Reloader;

    private ParticleSystem muzzleFireParticleSystem;


    private WeaponRecoil m_WeaponRecoil;
    private WeaponRecoil WeaponRecoil
    {
        get
        {
            if (m_WeaponRecoil == null)
                m_WeaponRecoil = GetComponent<WeaponRecoil>();
            return m_WeaponRecoil;
        }
    }



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

        bool isLocalPlayerControlled = AimTarget == null;

        if(!isLocalPlayerControlled)
            muzzle.LookAt(AimTarget.position + AimTargetOffset);


        Projectile newBullet = (Projectile) Instantiate(projectile, muzzle.position, muzzle.rotation);

        if (isLocalPlayerControlled)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPosition = ray.GetPoint(500);
            if(Physics.Raycast(ray, out hit))
                targetPosition = hit.point;

            newBullet.transform.LookAt(targetPosition + AimTargetOffset);
        }


        if (shouldRecoil && this.WeaponRecoil != null)
            this.WeaponRecoil.Activate();

        FireEffect();

        audioFire.Play();
        canFire = true;
    }
}
