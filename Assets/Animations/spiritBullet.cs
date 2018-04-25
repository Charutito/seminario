using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiritBullet : MonoBehaviour

{
    Animator _anim;
	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();

        float random = Random.value;

        _anim.SetFloat("randomHit",random);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
