using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Spells
{
    public class CollisionDamage : MonoBehaviour
    {
        public int Damage = 0;

        public bool DestroyOnCollision = false;

        public void SetDamage(int damage)
        {
            this.Damage = damage;
        }

        private void DealDamage(GameObject target)
        {
            var damageable = target.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(Damage, DamageType.Spell);
                
                if(DestroyOnCollision) Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            DealDamage(other.collider.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamage(other.gameObject);
        }
    }
}

