using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour {
    public GameObject drop;
    public Destructible dest;
    public int toDrop;
    bool hasDropped=false;
    public Transform DropPos;
    void Start () {

        dest = gameObject.GetComponent<Destructible>();
    }
    private void Update()
    {
        if (dest.currentHits == toDrop&&!hasDropped)
        {
            Drop();
            hasDropped = true;
        }
    }
    void Drop()
    {
        var lata = Instantiate(drop);
        lata.transform.position = DropPos.position;
    }
}
