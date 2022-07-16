using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoRenderer : MonoBehaviour
{
    [SerializeField] float sphereRadius, boxRadius;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.DrawWireCube(transform.position, new Vector3(boxRadius, boxRadius, boxRadius));
    }
}
