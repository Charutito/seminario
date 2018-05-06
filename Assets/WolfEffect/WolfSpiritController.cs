using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpiritController : MonoBehaviour {

    public Renderer rend;
    
    public Material newMaterialRef;
    public Shader dissolve;
    private float shininess = 0.7f;

    void Start () {
        if (rend != null)
            rend = GetComponent<Renderer>();

        rend.materials[1] = newMaterialRef;
        rend.materials[1].shader = dissolve;
    }	
	
	void Update () {
        shininess = Mathf.PingPong(Time.time, 1);
        rend.materials[1].SetFloat("_TotalOpacity", shininess);
    }
}
