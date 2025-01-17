﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
    public class CollisionDamage : MonoBehaviour
    {
        private SpellBehaviour _behaviour;
        private int _damage;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
            _damage = _behaviour.Definition.Damage;
        }

        private void DealDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(new Damage
                {
                    Amount = _damage,
                    Type = _behaviour.Definition.DamageType,
                    OriginPosition = transform.position,
                    OriginRotation = transform.rotation,
                    Displacement = _behaviour.Definition.DamageDisplacement
                });
                
                if(_behaviour.Definition.DestroyOnCollision) Destroy(gameObject);

                if (_behaviour.Definition.DamageMultiplier > 0) _damage = Mathf.RoundToInt(_damage * _behaviour.Definition.DamageMultiplier);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamage(other.gameObject);
        }
    }
}

