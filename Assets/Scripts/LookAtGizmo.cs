using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtGizmo : MonoBehaviour
{
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
