using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstacles : MonoBehaviour {
    public GameObject cam;

   

    void Update() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {

            if (hit.collider.gameObject.tag == "Player" )
            {
                cam.SetActive(false);
            }
            else if (hit.collider.gameObject.tag != "Player")
            {
                cam.SetActive(true);

            }
        }
      
	}
}
