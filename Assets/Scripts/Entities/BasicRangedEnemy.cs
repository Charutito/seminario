using FSM;
using Managers;
using UnityEngine;
using System;
using GameUtils;
using Util;
using BattleSystem;





namespace Entities
{
    public class BasicRangedEnemy : BasicEnemy
    {
        [Range(0f, 10f)]
        public float RangeToAim;        
        public float fireSpeed;
        public float recoilTime;
        public float nextFire;
        [Range(0f, 10f)]
        public float FleeTime;

        protected override void SetFsm()
        {
            EntityFsm = new BasicRangedEnemyFSM(this);            
        }     

        public Vector3 PosToFlee()
        {
            return transform.position - Target.transform.position * FleeTime;
        }

        public void Shot()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Vector3.Distance(transform.position, Target.transform.position))){
                if (hit.collider.gameObject.tag == "Player")
                {
                    var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(AttackDamage, DamageType.Attack);
                    }
                }

            }
            

            
    }
    }
}







