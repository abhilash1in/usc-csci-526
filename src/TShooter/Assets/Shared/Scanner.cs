using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{
    [SerializeField] float scanSpeed;
    [Range(0, 360)]
    [SerializeField] float fieldOfView;
    [SerializeField] LayerMask mask;
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
        print("ScanForTargets");
        List<T> targets = new List<T>();

        Collider[] results = Physics.OverlapSphere(transform.position, ScanRange);
        for (int i = 0; i < results.Length; i++)
        {
            var target = results[i].transform.GetComponent<T>();

            if (target == null)
                continue; 

            if (!IsInLineOfSight(Vector3.up, results[i].transform.position))
                continue;

            targets.Add(target);
        }

        PrepareScanner();
        return targets;
    }

    bool IsInLineOfSight(Vector3 eyeHeight, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        if(Vector3.Angle(transform.forward, direction.normalized) < (fieldOfView / 2))
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // Something blocking our view
            if(Physics.Raycast(transform.position + eyeHeight, direction.normalized, distanceToTarget, mask))
            {
                return false;
            }
            return true;
        }
        return false;
    }
}
