using BattleSystem;
using FSM;
using Steering;
using System;
using GameUtils;
using UnityEngine;
using Util;
using System.Collections.Generic;

namespace Entities
{
    public class ChargedEnemy : BasicEnemy
    {
        public float ChargeTime;
        public float recoveryTime;
        public event Action OnAttack = delegate { };
        public ColliderObserver attackArea;
        public Transform posToCharge;
        public Vector3 dashpos;

        public List<GameObject> chargeParticle;

        public Entity fakeCharacter;

        protected override void Start()
        {
            Target = fakeCharacter;
        }

        protected override void SetFsm()
        {
            EntityFsm = new ChargedEnemyFSM(this);          
        }

        public void ChargeAttack_HitInit()
        {
            attackArea.TriggerEnter += ChargeAttack_Damage;
            attackArea.gameObject.SetActive(true);
        }

        public void ChargeAttack_HitEnd()
        {
            attackArea.TriggerEnter -= ChargeAttack_Damage;
            attackArea.gameObject.SetActive(false);

            foreach (var item in chargeParticle)
            {
                item.SetActive(false);
            }
        }

        private void ChargeAttack_Damage(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                /*damageable.TakeDamage(new Damage
                {
                    amount = AttackDamage,
                    type = DamageType.Attack,
                    origin = transform,
                    originator = this
                });*/
            }
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsGod && (EntityFsm.Current.name ==  "Recovering" || EntityFsm.Current.name == "Stalking" || EntityFsm.Current.name == "Idling"))
            {
                base.TakeDamage(damage);
            }
        }

        public void Attack()
        {
            if (OnAttack!=null)
            {
                OnAttack();
            }
        }
    }
}