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
        }

        #region Components
        private string attackAnimation = string.Empty;
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
            #endregion

            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Charging, Charge)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Charge)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Stalk)
                .SetTransition(Trigger.Attack, Charge)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Recover, Recovering)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Recovering)
                .SetTransition(Trigger.Charging, Charge)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Die, Death);
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
           };
            Charge.OnExit += () =>
            {
            };
            #endregion

            #region Attack State
            Attack.OnEnter += () =>
            {
                entity.posToCharge = entity.Target.transform;
                var dashPosition = entity.posToCharge.position +
                                   entity.transform.forward * entity.transform.GetMaxDistance();
                entity.EntityMove.SmoothMoveTransform(dashPosition, 0.5f,() => Feed(Trigger.Recover));
            };
        
            #endregion

            #region Recover State
            Recovering.OnEnter += () =>
            {
                entity.Animator.SetBool(Animations.Recovering,true);
            };
            Recovering.OnUpdate += () =>
            {
                FrameUtil.AfterDelay(entity.recoveryTime, () =>
                {
                    Feed(Trigger.Stalking);
                });
            };
            Recovering.OnExit += () =>
            {
                entity.Animator.SetBool(Animations.Recovering, false);

            };
            #endregion

            #region Entity Events
            entity.OnAttack += OnAttack;
            entity.OnCrash += OnCrash;
            entity.OnAttackRecovered += OnAttackRecover;
            entity.OnSetAction += OnSetAction;
            entity.OnTakeDamage += OnTakingDamage;

            entity.OnDeath += (e) =>
            {
                entity.OnAttack -= OnAttack;
                entity.OnCrash -= OnCrash;
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

        private void OnCrash()
        {
            Feed(Trigger.Recover);
        }

        private void OnAttackRecover()
        {
            entity.IsAttacking = false;
            entity.CurrentAction = GroupAction.Stalking;
        }

        private void OnTakingDamage(int damage, DamageType type)
        {
            entity.HitFeedback();          
        }

        private void OnSetAction(GroupAction newAction, GroupAction lastAction)
        {
            if (newAction == GroupAction.Attacking)
            {
                attackAnimation = Animations.Charging;
                entity.IsAttacking = true;
                Feed(Trigger.Charging);
            }
            else if (newAction == GroupAction.Stalking)
            {
                Feed(Trigger.Stalking);
            }

        }

    }
    }
