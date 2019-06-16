using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeToLive;
    [SerializeField] float damage;


    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (GameManager.Instance.isNetworkGame)
            {
                if (hit.collider.tag == "Player")
                {
                    NetworkPlayerDestructable des = hit.collider.GetComponent<NetworkPlayerDestructable>();
                    des.Shoot(hit.collider.name, damage);
                }
                else if (hit.collider.tag == "Base")
                {
                    Debug.Log("lapalapalapalapalapalapalapalapa");
                    NetworkBaseDestructable des = hit.collider.GetComponent<NetworkBaseDestructable>();
                    des.ShootBase(hit.collider.name, damage);
                }

            }
            else
            {
                CheckDestructable(hit.transform);
            }

        }
    }

    private void CheckDestructable(Transform other)
    {
        print("Hit: " + other.name);
        var destructable = other.GetComponent<Destructable>();
        if(destructable == null)
        {
            return;
        }
        destructable.TakeDamage(damage);
    }
}
