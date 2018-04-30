using System;
using BattleSystem;
using Entities;
using UnityEngine;

namespace AnimatorFSM
{
    public class BasicEnemyStateManager : MonoBehaviour
    {
        public BasicEnemy Entity { get; private set; }
        public Animator FSM { get; private set; }

        private int _currentHitsToStun;

        private void Awake()
        {
            Entity = GetComponentInParent<BasicEnemy>();
            FSM = GetComponent<Animator>();
        }

        private void Start()
        {
            Entity.OnDeath += entity =>
            {
                FSM.SetTrigger("Death");
            };

            Entity.OnTakeDamage += (amount, type) =>
            {
                _currentHitsToStun++;
                Entity.HitFeedback();

                if (type == DamageType.Block || _currentHitsToStun >= Entity.hitsToGetStunned)
                {
                    FSM.SetTrigger("Stun");
                }
                else if (type == DamageType.ThirdAttack)
                {
                    FSM.SetTrigger("KnockBack");
                }
                else
                {
                    FSM.SetTrigger("GetHit");
                }
            };

            Entity.OnSetAction += (newAction, lastAction) =>
            {
                switch (newAction)
                {
                    case GroupAction.Attacking:
                    case GroupAction.SpecialAttack:
                        FSM.SetTrigger("Attack");
                        break;
                    case GroupAction.Stalking:
                        FSM.SetBool("Activated", true);
                        break;
                }
            };
        }

        private void Update()
        {
            FSM.SetBool("TargetInAttackRange", Vector3.Distance(Entity.transform.position, Entity.Target.transform.position) <= Entity.AttackRange);
        }
    }
}
