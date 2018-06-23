using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour {
    public GameObject drop;
    public Destructible dest;
    public int toDrop;
    public Transform DropPos;
    void Start () {

        dest = gameObject.GetComponent<Destructible>();

    }
    private void Update()
    {
        if (dest.currentHits >= toDrop)
        {
            Drop();            
        }
    }
    void Drop()
    {
        dest.currentHits = 0;
        var lata = Instantiate(drop);
        lata.transform.position = DropPos.position;
    }
}
