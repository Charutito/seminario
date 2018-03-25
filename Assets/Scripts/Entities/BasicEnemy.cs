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

        public override void TriggerAttack()
        {
            throw new System.NotImplementedException();
        }

        public override void TriggerSpecialAttack()
        {
            throw new System.NotImplementedException();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, 0, 2));
        
        }
    }
}
