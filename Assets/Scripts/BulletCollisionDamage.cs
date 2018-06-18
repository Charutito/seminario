using FSM;
using Managers;
using UnityEngine;
using System;
using GameUtils;
using Util;
using System.Collections;
using BattleSystem;

public class BulletCollisionDamage : MonoBehaviour
{
    public int Damage;
    
    private void OnTriggerEnter(Collider col)
    {
        var damageable = col.gameObject.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.TakeDamage(new Damage
            {
                Amount = Damage,
                Type = DamageType.Attack,
                OriginPosition = transform.position,
                OriginRotation = transform.rotation
            });
            
            Destroy(this.gameObject);
        }
    }
}
