using BattleSystem;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        [Header("Attack")]
        public float attackDelay = 2f;
        public float attackRange = 1.5f;

        private SkeletonFSM fsm;
        private NavMeshAgent agent;

        public override void TakeDamage(int damage, DamageType type)
        {
            base.TakeDamage(damage, type);
        }

        protected override void OnUpdate()
        {
            fsm.Update();

            if (!IsDead)
            {
                Animator.SetFloat("Velocity Z", Vector3.Project(agent.desiredVelocity, transform.forward).magnitude);
            }
        }

        private void Start()
        {
            fsm = new SkeletonFSM(this);
            agent = GetComponent<NavMeshAgent>();

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
