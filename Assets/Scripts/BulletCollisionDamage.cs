using FSM;
using Managers;
using UnityEngine;
using System;
using GameUtils;
using Util;
using System.Collections;
using BattleSystem;

public class BulletCollisionDamage : MonoBehaviour {

    public int damage;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            var damageable = col.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage , DamageType.Attack);
                Destroy(this.gameObject);
            }
        }
    }
}
