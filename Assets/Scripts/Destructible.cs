using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    public GameObject drop;
    public GameObject particles;

    public void TakeDamage(int damage, DamageType type = DamageType.Unknown)
    {
        var parts = Instantiate(particles);
        parts.transform.position = transform.position;
        Destroy(parts, 1);
        
        var toDrop = Instantiate(drop);
        toDrop.transform.position = transform.position;
        Destroy(gameObject);
    }
}
