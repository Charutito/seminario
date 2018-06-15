using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pendulum : MonoBehaviour {
    Transform current;
    public float Time;
    public Transform[] positions;
    public ParticleSystem fafafa;

    public void Start()
    {
        current = positions.First();      
    }

    void Update () {
        if (Vector3.Distance(transform.position,current.position) <=0.1f)
        {
            current = positions.Where(x => x != current).Skip(Random.Range(0,positions.Count())).First();
        }

        
        transform.position = Vector3.Lerp(transform.position, current.position, Time);
	}
}
