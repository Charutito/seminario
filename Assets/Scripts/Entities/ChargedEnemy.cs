using BattleSystem;
using FSM;
using Steering;
using System;
using GameUtils;
using UnityEngine;
using Util;

namespace Entities
{
    public class ChargedEnemy : BasicEnemy
    {
        public float ChargeTime;
        public float recoveryTime;
        public event Action OnAttack = delegate { };
        public event Action OnCrash = delegate { };
        public ColliderObserver attackArea;
        public Transform posToCharge;
        public Vector3 dashpos;

        private Entity _entity;

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
        }

        private void ChargeAttack_Damage(Collider other)
        {
            var character = other.GetComponent<CharacterEntity>();
            var destructible = other.GetComponent<Destructible>();

            if (destructible != null)
            {
                destructible.destroy();
            }
            if (character != null)
            {
                _entity = GetComponent<Entity>();
                character.DmgDdisp(transform.forward);
                character.TakeDamage(_entity.AttackDamage, DamageType.Attack);
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