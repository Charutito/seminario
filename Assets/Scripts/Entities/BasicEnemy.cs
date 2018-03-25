using Entities;
using FSM;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

namespace Entities
{
    public class BasicEnemy : GroupEntity
    {
        private BasicEnemyFSM fsm;
        public GameObject Hitpart;
        public Transform hitpos;

        private void Start()
        {
            Target = GameManager.Instance.Character;
            fsm = new BasicEnemyFSM(this);
        }

        protected override void OnUpdate()
        {
            fsm.Update();
        }

        public override void TriggerAttack()
        {
            throw new System.NotImplementedException();
        }

        public override void TriggerSpecialAttack()
        {
            throw new System.NotImplementedException();
        }
        public override void TakeDamage(int damage, DamageType type)
        {
            base.TakeDamage(damage, type);
            var part = Instantiate(Hitpart, hitpos.position, hitpos.rotation, hitpos);
            Destroy(part, 1);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, 0, 2));
        
        }
    }
}
