using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    public GameObject drop;
    public GameObject particles;
    public AudioEvent DestroyAudio;

    public void TakeDamage(Damage damage)
    {
        Instantiate(drop, transform.position + Vector3.up, Quaternion.identity);
        DestroyAudio.PlayAtPoint(transform.position);
        
        var parts = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(parts, 1);
        Destroy(gameObject);
    }
}
