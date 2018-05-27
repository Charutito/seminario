using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public GameObject particle;
    public AudioEvent CollectSound;

    protected abstract void Collect();

    protected virtual void SelfDestruct(Transform other)
    {
        CollectSound.PlayAtPoint(transform.position);
            
        var parts = Instantiate(particle, other.position, Quaternion.identity, other);
        Destroy(parts, 1.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collect();
        SelfDestruct(other.transform);
    }
}
