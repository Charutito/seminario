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

        public bool StateLocked { get; set; }

        private int _currentHitsToStun;

        private void SetState(string state, bool force = false)
        {
            if(!StateLocked || force) FSM.SetTrigger(state);
        }

        private void Awake()
        {
            Entity = GetComponentInParent<BasicEnemy>();
            FSM = GetComponent<Animator>();
        }

        private void Start()
        {
            Entity.OnDeath += entity =>
            {
                SetState("Death", true);
            };

            Entity.OnTakeDamage += (amount, type) =>
            {
                _currentHitsToStun++;
                Entity.HitFeedback();

                if (type == DamageType.Block || _currentHitsToStun >= Entity.hitsToGetStunned)
                {
                    SetState("Stun");
                    _currentHitsToStun = 0;
                }
                else if (type == DamageType.ThirdAttack)
                {
                    SetState("KnockBack", true);
                }
                else if (type == DamageType.FlyUp)
                {
                    SetState("FlyUp");
                }
                else 
                {
                    SetState("GetHit");
                }
            };

            Entity.OnSetAction += (newAction, lastAction) =>
            {
                switch (newAction)
                {
                    case GroupAction.Attacking:
                    case GroupAction.SpecialAttack:
                        SetState("Attack");
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
