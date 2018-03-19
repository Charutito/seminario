using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {
    public Transform player;
    public float distToLit;
    public bool islit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position,player.position)<=distToLit && !islit)
        {
            lit();
        }else if(Vector3.Distance(transform.position, player.position) >= distToLit && islit)
        {
            unlit();
        }
	}
    void lit()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_ASEOutlineWidth",0.1f);
        islit = true;
    }
    void unlit()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_ASEOutlineWidth", 0f);
        islit = false;


    }
}
