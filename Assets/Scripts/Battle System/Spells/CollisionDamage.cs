using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
    public class CollisionDamage : MonoBehaviour
    {
        private SpellBehaviour _behaviour;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
        }

        private void DealDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_behaviour.Definition.Damage, DamageType.Spell);
                
                if(_behaviour.Definition.DestroyOnCollision) Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamage(other.gameObject);
        }
    }
}

