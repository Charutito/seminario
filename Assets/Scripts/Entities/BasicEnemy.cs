using Entities;
using FSM;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class BasicEnemy : GroupEntity
    {
        [Range(1f, 5f)]
        public float AttackRange = 2f;

        private BasicEnemyFSM fsm;

        private void Start()
        {
            Target = GameManager.Instance.Character;
            fsm = new BasicEnemyFSM(this);
        }

        protected override void OnUpdate()
        {
            fsm.Update();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}
