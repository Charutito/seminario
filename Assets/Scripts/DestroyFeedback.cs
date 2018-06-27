using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeedback : MonoBehaviour {

    public GameObject Particles;
    public GameObject Decal;

    private void OnDestroy()
    {
        if (Particles!=null)
        {
            var explosion = Instantiate(Particles);
            explosion.transform.position = transform.position;
        }
        if (Decal!=null)
        {
            var FloorMark = Instantiate(Decal);
            FloorMark.transform.position = transform.position;
        }
    }


}
