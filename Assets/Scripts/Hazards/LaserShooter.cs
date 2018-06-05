using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour {
    public float FireRate;
    float nextfire;
    public GameObject Prefab;
    public Transform canon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > nextfire)
        {
            Fire();
            nextfire = Time.time + FireRate;
        }
    }
    void Fire()
    {
        var bullet = Instantiate(Prefab);
        bullet.transform.position = canon.transform.position ;
        bullet.transform.rotation = canon.transform.rotation;
    }
}
