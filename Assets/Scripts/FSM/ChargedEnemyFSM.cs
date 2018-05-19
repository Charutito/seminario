using System.Runtime.InteropServices;
using BattleSystem;
using Entities;
using UnityEngine;
using Util;

namespace FSM
{
    public class ChargedEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int Die = 2;
            public static int Recover = 3;
            public static int Charging = 4;
            public static int Stalking = 5;
            public static int GetHit = 6;
        }

        private class Animations
        {
            public static string Attack = "Attack";
            public static string Recovering = "Recovering";
            public static string ChargeAttack = "ChargeAttack";
            public static string Death = "Death";
            public static string Relax = "Relax";
            public static string RandomDeath = "RandomDeath";
            public static string Move = "Velocity Z";
            public static string Charging = "Charging";
            public static string GetHit = "GetHit";
        }

        #region Components
        private ChargedEnemy entity;
        #endregion

        public ChargedEnemyFSM(ChargedEnemy entity)
        {
            this.debugName = "ChargedEnemyFSM";
            this.entity = entity;

            #region States Definitions
            State<int> Idle = new State<int>("Idling");
            State<int> Charge = new State<int>("Charging");
            State<int> Stalk = new State<int>("Stalking");
            State<int> Attack = new State<int>("Attacking");
            State<int> Death = new State<int>("Death");
            State<int> Recovering = new State<int>("Recovering");
            State<int> GetHit = new State<int>("GetHit");
            #endregion

            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Charging, Charge)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Charge)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Stalk)
                .SetTransition(Trigger.Attack, Charge)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Recover, Recovering)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Recovering)
                //.SetTransition(Trigger.Charging, Charge)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(GetHit)
               .SetTransition(Trigger.Recover, Recovering)
               .SetTransition(Trigger.Stalking, Stalk)
               .SetTransition(Trigger.Die, Death)
               .SetTransition(Trigger.GetHit, GetHit);
            #endregion

            #region idle State
            Idle.OnEnter += () =>
            {
                entity.Animator.SetBool(Animations.Relax, true);
                entity.Animator.SetFloat(Animations.Move, 0f);
            };
            Idle.OnUpdate += () =>
            {
                if (true)
                {
                    Feed(Trigger.Stalking);
                }
            };
            Idle.OnExit += () =>
            {
                entity.Animator.SetBool(Animations.Relax, false);
            };
            #endregion

            #region Stalk State 
            var nextLocation = Vector3.zero;
            var normalSpeed = 1f;

            Stalk.OnEnter += () =>
            {
                normalSpeed = entity.Agent.speed;
                entity.Agent.speed = 0.3f;
            };

            Stalk.OnUpdate += () =>
            {
                if (entity.EntityMove.HasAgentArrived())
                {
                    var newLocation = Random.insideUnitCircle * 4;
                    nextLocation = entity.transform.position + new Vector3(newLocation.x, entity.transform.position.y, newLocation.y);
                    entity.EntityMove.MoveAgent(nextLocation);
                }
                entity.EntityMove.RotateInstant(entity.Target.transform.position);
                if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) <= entity.AttackRange)
                {
                    Feed(Trigger.Attack);
                }
            };
            Stalk.OnExit += () =>
            {
                entity.Agent.isStopped = true;
                entity.Agent.speed = normalSpeed;
            };
            #endregion

            #region Charge State
            Charge.OnEnter += () =>
            {
                entity.Animator.SetTrigger(Animations.Charging);

                foreach (var item in entity.chargeParticle)
                {
                    item.SetActive(true);
                }
            };
            Charge.OnUpdate += () =>
            {
                entity.EntityMove.RotateTowards(entity.Target.transform.position);
            };
            Charge.OnExit += () =>
            {
                entity.posToCharge = entity.Target.transform;
                entity.dashpos = entity.posToCharge.position +
                                   entity.transform.forward * entity.transform.GetMaxDistance(entity.transform.forward);
                entity.EntityMove.RotateInstant(entity.dashpos);
            };
            #endregion

            #region Attack State
            Attack.OnEnter += () =>
            {
                //entity.EntityMove.SmoothMoveTransform(dashPosition, 0.5f,() => Feed(Trigger.Recover));
                entity.EntityMove.ConstantMoveTransform(entity.dashpos, 4, () => Feed(Trigger.Recover));
            };
            Attack.OnExit += () =>
            {
                //entity.Animator.SetBool(Animations.Recovering, true);
            };          
            #endregion

            #region Recover State
            Recovering.OnEnter += () =>
            {
                entity.Animator.SetBool(Animations.Recovering, true);

                FrameUtil.AfterDelay(entity.recoveryTime, () =>
                {
                    Feed(Trigger.Stalking);
                });
            };
            Recovering.OnUpdate += () =>
            {

            };
            Recovering.OnExit += () =>
            {
                entity.Animator.SetBool(Animations.Recovering, false);
            };
            #endregion

            #region GetHit State

            GetHit.OnEnter += () =>
            {
                entity.Animator.SetTrigger(Animations.GetHit);
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;

                FrameUtil.AfterDelay(entity.getHitDuration, () =>
                {
                    Feed(Trigger.Recover);
                });
            };
            #endregion

            #region Death State
            Death.OnEnter += () =>
            {
                entity.Animator.SetBool("Death", false);
                entity.Animator.SetTrigger("TriggerDeath");
                entity.Animator.SetInteger("RandomDeath", Random.Range(0, 3));

                entity.Agent.enabled = false;
                entity.Collider.enabled = false;
            };
            #endregion

            #region Entity Events
            entity.OnAttack += OnAttack;
            entity.OnAttackRecovered += OnAttackRecover;
            entity.OnSetAction += OnSetAction;
            entity.OnTakeDamage += OnTakingDamage;

            entity.OnDeath += (e) =>
            {
                entity.OnAttack -= OnAttack;
                entity.OnAttackRecovered -= OnAttackRecover;
                entity.OnSetAction -= OnSetAction;
                entity.OnTakeDamage -= OnTakingDamage;
                Feed(Trigger.Die);
            };
            #endregion
        }

        private void OnAttack()
        {
            Feed(Trigger.Attack);           
        }

        private void OnAttackRecover()
        {
            entity.IsAttacking = false;
            entity.CurrentAction = GroupAction.Stalking;
        }

        private void OnTakingDamage(Damage damage)
        {   
            if (entity.EntityFsm.Current.name == "Recovering" || entity.EntityFsm.Current.name == "Stalking" || entity.EntityFsm.Current.name == "Idling")
            {
                entity.HitFeedback();
                Feed(Trigger.GetHit);
            }
        }

        private void OnSetAction(GroupAction newAction, GroupAction lastAction)
        {
            if (newAction == GroupAction.Attacking)
            {
                entity.IsAttacking = true;
                Feed(Trigger.Attack);
            }
            else if (newAction == GroupAction.Stalking)
            {
                Feed(Trigger.Stalking);
            }
        }

    }
    }
