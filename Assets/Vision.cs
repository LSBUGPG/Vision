using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Alert))]
public class Vision : MonoBehaviour
{
    public SphereCollider viewDistance;
	Alert alert;
    public float fov = 90.0f;

	void Awake()
	{
		alert = GetComponent<Alert>();
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, fov, viewDistance.radius, 0.5f, 1.0f);
        Gizmos.DrawWireSphere(Vector3.zero, viewDistance.radius);
    }

    void OnTriggerStay(Collider thing)
    {
        if (thing.CompareTag("Player"))
        {
            Vector3 vectorToPlayer = thing.transform.position - transform.position;
            if (Vector3.Angle(vectorToPlayer, transform.forward) < 0.5f * fov)
            {
				alert.enabled = true;
            }
        }
    }
}
