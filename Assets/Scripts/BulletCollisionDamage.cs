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
                amount = Damage,
                type = DamageType.Attack,
                origin = transform
            });
            
            Destroy(this.gameObject);
        }
    }
}
