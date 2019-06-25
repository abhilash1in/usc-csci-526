using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeToLive;
    [SerializeField] float damage;
    [SerializeField] Transform bulletHole;
    Vector3 destination;
    public CustomNetworkBehviour.ETeamID originTeamID;


    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        if (isDetinationReached())
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(destination != Vector3.zero)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            CheckDestructable(hit);
        }
    }

    private void CheckDestructable(RaycastHit hitInfo)
    {
        print("Hit: " + hitInfo.transform.name);
        var destructable = hitInfo.transform.GetComponent<Destructable>();

        destination = hitInfo.point + hitInfo.normal * 0.0015f;

        Transform hole = (Transform) Instantiate(bulletHole, destination, Quaternion.LookRotation(hitInfo.normal) * Quaternion.Euler(0, 180f, 0));
        hole.SetParent(hitInfo.transform);

        if(destructable == null)
        {
            destructable = hitInfo.transform.GetComponentInParent<Destructable>();
            if (destructable == null)
            {
                return;
            }
        }
        if(destructable.TeamID != originTeamID)
        {
            HelperMethods.ShowMessage("Nice Shot!");
            destructable.TakeDamage(damage);
        }
            
        else
        {
            HelperMethods.ShowMessage("Frendly Fire");
        }
    }

    bool isDetinationReached()
    {
        if (destination == Vector3.zero)
            return false;

        Vector3 directionToDestination = destination - transform.position;

        float dot = Vector3.Dot(directionToDestination, transform.forward);

        if (dot < 0)
            return true; 

        return false;
    }
}
