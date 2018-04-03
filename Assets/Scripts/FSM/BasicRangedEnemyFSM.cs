using BattleSystem;
using Entities;
using UnityEngine;
using Util;

namespace FSM
{

    public class BasicRangedEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int Aim = 2;
            public static int Die = 3;
            public static int Stun = 4;
            public static int Idle = 5;

        }
        private class Animations
        {
            public static string Attack = "Attack";
            public static string SpecialAttack = "SpecialAttack";
            public static string Death = "Death";
            public static string Aim = "Aim";
            public static string Relax = "Relax";
            public static string RandomDeath = "RandomDeath";
            public static string Countered = "Countered";
            public static string Move = "Velocity Z";
        }

        #region Components
        private BasicRangedEnemy entity;
        #endregion

        #region Local Vars
        private string attackAnimation = string.Empty;
        private int currentHitsToStun = 0; // Tendria que reducirse a lo largo del tiempo si no recibe ataques
        #endregion
        public BasicRangedEnemyFSM(BasicRangedEnemy entity)
        {
            this.debugName = "BasicFSM";
            this.entity = entity;
            #region States Definitions
            State<int> Idle = new State<int>("Idling");
            State<int> aim = new State<int>("Aiming");
            State<int> Follow = new State<int>("Following");
            State<int> Attack = new State<int>("Attacking");
            State<int> Death = new State<int>("Death");
            State<int> Stunned = new State<int>("Stunned");
            #endregion
            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death)
                .SetTransition(Trigger.Aim, aim);


            StateConfigurer.Create(aim)
                 .SetTransition(Trigger.Idle, Idle)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Follow)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Aim, aim)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Aim, aim)
                .SetTransition(Trigger.Die, Death);
            #endregion
            #region idle State
            Idle.OnEnter += () =>
            {
                entity.Animator.SetBool(Animations.Relax, true);
            };
            Idle.OnUpdate += () =>
            {                
                if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) <= entity.RangeToAim)
                {
                    Feed(Trigger.Aim);
                }
            };
            Idle.OnExit += () =>
            {
                entity.Animator.SetBool(Animations.Relax, false);
            };
            #endregion

            #region Aim State
            aim.OnEnter += () =>
            {
                entity.Animator.SetBool(Animations.Aim, true);
            };
            aim.OnUpdate += () =>
            {
                entity.EntityMove.RotateTowards(entity.Target.transform.position);
                if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) >= entity.RangeToAim)
                {
                    Feed(Trigger.Idle);
                }
            };
            aim.OnExit += () =>
            {
                entity.Animator.SetBool(Animations.Aim, false);
            };
            #endregion
            #region Attack State
            Attack.OnEnter += () =>
            {
                entity.Animator.SetTrigger(attackAnimation);
            };
            #endregion
            #region Death State
            Death.OnEnter += () =>
            {
                entity.Animator.SetTrigger("Death");
                entity.Animator.SetInteger("RandomDeath", Random.Range(0, 3));

                entity.Agent.enabled = false;
                entity.Collider.enabled = false;
            };
            #endregion
            #region Stunned State
            Stunned.OnEnter += () =>
            {
                currentHitsToStun = 0;
                entity.Animator.SetTrigger(Animations.Countered);
                FrameUtil.AfterDelay(entity.stunDuration, () =>
                {
                    Feed(Trigger.Idle);
                });
            };
            #endregion
            #region Entity Events
            entity.OnAttackRecovered += OnAttackRecover;
            entity.OnSetAction += OnSetAction;
            entity.OnTakeDamage += OnTakingDamage;

            entity.OnDeath += (e) =>
            {
                entity.OnAttackRecovered -= OnAttackRecover;
                entity.OnSetAction -= OnSetAction;
                entity.OnTakeDamage -= OnTakingDamage;
                Feed(Trigger.Die);
            };
            #endregion



        }
        private void OnAttackRecover()
        {
            entity.IsAttacking = false;
            entity.CurrentAction = GroupAction.Stalking;
        }

        private void OnTakingDamage(int damage, DamageType type)
        {
            entity.HitFeedback();

            currentHitsToStun++;

            if (type == DamageType.Block || currentHitsToStun >= entity.hitsToGetStunned)
            {
                Feed(Trigger.Stun);
            }
        }

        private void OnSetAction(GroupAction newAction, GroupAction lastAction)
        {
            if (newAction == GroupAction.Attacking)
            {
                attackAnimation = Animations.Attack;
                entity.IsAttacking = true;
                Feed(Trigger.Attack);
            }
            else if (newAction == GroupAction.SpecialAttack)
            {
                attackAnimation = Animations.SpecialAttack;
                Feed(Trigger.Attack);
            }
            else if (newAction == GroupAction.Stalking)
            {
                Feed(Trigger.Aim);
            }
        }
    }

}
   