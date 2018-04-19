using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    public GameObject drop;
    public GameObject particles;
    // Use this for initialization

    public void destroy()
    {
        var parts = Instantiate(particles);
        parts.transform.position = transform.position;
        Destroy(parts, 1);
        var toDrop = Instantiate(drop);
        toDrop.transform.position = transform.position;
        Destroy(this.gameObject);
    }
}
