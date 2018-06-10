using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerLaser : MonoBehaviour {
    public float FireRate;
    public float PauseRate;

    float NextOn;
    float nextOf;
    bool firing;
    public GameObject Laser;



    void Update()
    {

        if (Time.time > NextOn)
        {
            Fire();
            NextOn = Time.time + FireRate;
        }
       
    }
    void Fire()
    {
        if (firing)
        {
            Laser.SetActive(false);
            firing = false;
        }
        else
        {
            Laser.SetActive(true);
            firing = true;
        }
    }

}
