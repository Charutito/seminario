using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMoveAndDestroy : MonoBehaviour {

	void Update () {
        this.transform.position += transform.forward * Time.deltaTime * 7;
        Destroy(this.gameObject, 0.5f);
	}
}
