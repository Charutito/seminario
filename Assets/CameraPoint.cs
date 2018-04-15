using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour {
    public float Range;
	// Use this for initialization
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
