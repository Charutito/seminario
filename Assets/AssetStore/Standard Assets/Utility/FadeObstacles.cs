using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstacles : MonoBehaviour {
    public GameObject target;
    public GameObject current;

   

    void Update() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {

            if (hit.collider.gameObject.tag == "Ambient" && hit.collider.gameObject!= current)
            {
                current = hit.collider.gameObject;
                var c = current.GetComponent<Renderer>().material.color;
                c.a = 0.5f;
                current.GetComponent<MeshRenderer>().material.color = c;
            }
            else if (hit.collider.gameObject.tag != "Ambient" && current != null)
            {
                var c2 = current.GetComponent<Renderer>().material.color;
                c2.a = 1f;
                current.GetComponent<MeshRenderer>().material.color = c2;
                current = null;
            }           
        }
      
	}
}
