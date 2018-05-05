using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class spiritBullet : MonoBehaviour

{
    Animator _anim;


	// Use this for initialization
	void Start () {
      
        
        _anim.SetInteger("RandomAttack", Random.Range(0,2));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
