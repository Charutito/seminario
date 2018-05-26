using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMoveAndDestroy : MonoBehaviour {

    public float speed = 7;
    public float lifetime =0.5f;

	void Update () {
        this.transform.position += transform.forward * Time.deltaTime * speed;
        Destroy(this.gameObject, lifetime);
	}
}
