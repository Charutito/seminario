﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public GameObject particle;
    public AudioEvent CollectSound;

    protected abstract void Collect();

    protected virtual void SelfDestruct()
    {
        CollectSound.PlayAtPoint(transform.position);
            
        var parts = Instantiate(particle);
        parts.transform.position = transform.position;
        Destroy(parts, 1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect();
        SelfDestruct();
    }
}
