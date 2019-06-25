using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Extensions;
using System;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{
    [SerializeField] float scanSpeed;
    [Range(0, 360)]
    [SerializeField] float fieldOfView;
    [SerializeField] public LayerMask mask;
    SphereCollider rangeTrigger;

    public float ScanRange
    {
        get
        {
            if (rangeTrigger == null)
                rangeTrigger = GetComponent<SphereCollider>();
            return rangeTrigger.radius;
        }
    }

    public event System.Action OnScanReady ;

    void PrepareScanner()
    {
        GameManager.Instance.Timer.Add(() => {
            if (OnScanReady != null)
                OnScanReady();
        }, scanSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(fieldOfView / 2) * GetComponent<SphereCollider>().radius);
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(-fieldOfView / 2) * GetComponent<SphereCollider>().radius);
    }


    Vector3 GetViewAngle(float angle)
    {
        float radian = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }


    public List<T> ScanForTargets<T>()
    {
        List<T> targets = new List<T>();

        Collider[] results = Physics.OverlapSphere(transform.position, ScanRange);
        // print("Colliding targets: " + results.Length);
        for (int i = 0; i < results.Length; i++)
        {
            var target = results[i].transform.GetComponent<T>();

            if (target == null)
                continue;

            // print("Found potential target");
            if (!transform.IsInLineOfSight(results[i].transform.position, fieldOfView, mask, Vector3.up))
            {
                // print("Potential target not in line of sight. Abandoning.");
                continue;
            }
                

            targets.Add(target);
        }

        PrepareScanner();
        return targets;
    }
}
