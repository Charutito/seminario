using BattleSystem;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class SkeletonEntity : Entity
    {
        [Header("Patrol")]
        public float patrolMinWaitTime = 4f;
        public float patrolMaxWaitTime = 8f;
        public float patrolRadius = 8f;

        [Header("Scout")]
        public float scoutTime = 4f;
        public float scoutSpeed = 2f;
        public float scoutMaxAngle = 5f;

        private SkeletonFSM fsm;        

        protected override void OnUpdate()
        {
            fsm.Update();
        }

        private void Start()
        {
            fsm = new SkeletonFSM(this);

            Stats.Health.OnActualChange += (float old, float current) =>
            {
                if (current == Stats.Health.Min)
                {
                    Destroy(gameObject);
                }
            };
        }
    }
}
