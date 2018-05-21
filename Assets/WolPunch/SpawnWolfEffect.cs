using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWolfEffect : MonoBehaviour {

    public GameObject wolf;

	public void SpawnWolf()
    {
        Instantiate(wolf,this.transform.position, transform.rotation);        
    }

}
