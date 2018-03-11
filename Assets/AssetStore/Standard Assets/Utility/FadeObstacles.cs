using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObstacles : MonoBehaviour {
    public GameObject target;
    GameObject current;
    void Update() {
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.gameObject.tag == "Ambient"&& hit.collider.gameObject!= current)
            {
                current = hit.collider.gameObject;
                current.GetComponent<Renderer>().material.SetFloat("_TotalOpacity", 0.5f);
            }
            else if (hit.collider.gameObject.tag != "Ambient" && current != null)
            {
                current.GetComponent<Renderer>().material.SetFloat("_TotalOpacity", 1f);
                current = null;
            }           
        }
      
	}
}
