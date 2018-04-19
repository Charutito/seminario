using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public GameObject particle;
    // Use this for initialization
    private void OnDestroy()
    {
        var parts = Instantiate(particle);
        parts.transform.position = transform.position;
        Destroy(particle, 1);
    } 
}
