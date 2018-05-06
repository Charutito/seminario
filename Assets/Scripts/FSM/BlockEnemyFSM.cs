using System.Runtime.InteropServices;
using BattleSystem;
using Entities;
using UnityEngine;
using Util;

namespace FSM
{
    public class BlockEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int Stalking = 2;
            public static int Block = 3;
            public static int Die = 4;
            public static int Stun = 5;
        }

        /// <summary>
        /// Variables and triggers in animator
        /// </summary>
        private class Animations
        {
            public static string Attack         = "Attack";
            public static string BlockAttack    = "BlockAttack";
            public static string SpecialAttack  = "SpecialAttack";
            public static string Death          = "Death";
            public static string RandomDeath    = "RandomDeath";
            public static string Countered      = "Countered";
            public static string Move           = "Velocity Z";
        }
        
        #region Components
        private BlockEnemy entity;
        private bool weakPointHit = false;
        #endregion

        public BlockEnemyFSM(BlockEnemy entity)
        {
            this.debugName = "BlockEnemyFSM";
            this.entity = entity;
            
            #region States Definitions
            State<int> Idle = new State<int>("Idling");
            State<int> Stalk = new State<int>("Stalking");
            State<int> Block = new State<int>("Blocking");
            State<int> Follow = new State<int>("Following");
            State<int> Attack = new State<int>("Attacking");
            State<int> Death = new State<int>("Death");
            State<int> Stunned = new State<int>("Stunned");
            #endregion

            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Block, Block)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Stalk)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Block, Block)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);
            
            StateConfigurer.Create(Block)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Follow)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Block, Block)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Block, Block)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Block, Block)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Die, Death);
            #endregion

            #region Stalk State
            Stalk.OnUpdate += () =>
            {
                entity.EntityMove.RotateTowards(entity.Target.transform.position, 1f);
            };
            #endregion
            
            #region Block State
            
            Block.OnEnter += () =>
            {
                entity.IsBlocking = true;
            };
            
            Block.OnUpdate += () =>
            {
                if (weakPointHit)
                {
                    weakPointHit = false;
                    entity.EntityMove.RotateInstant(entity.Target.transform.position);
                }
                else
                {
                    entity.EntityMove.RotateTowards(entity.Target.transform.position, 0.8f);
                }
            };
            
            Block.OnExit += () =>
            {
                entity.IsBlocking = false;
            };
            #endregion

            #region Follow State
            Follow.OnEnter += () =>
            {
                entity.Animator.SetFloat(Animations.Move, 1);
            };

            Follow.OnUpdate += () =>
            {
                entity.EntityMove.RotateInstant(entity.Target.transform.position);
                entity.EntityMove.MoveAgent(entity.Target.transform.position);

                if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) <= entity.AttackRange)
                {
                    Feed(Trigger.Attack);
                }
            };

            Follow.OnExit += () =>
            {
                entity.Animator.SetFloat(Animations.Move, 0);
            };
            #endregion


            #region Attack State
            Attack.OnEnter += () =>
            {
                entity.Animator.SetTrigger(Animations.SpecialAttack);
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
                entity.Animator.SetTrigger(Animations.Countered);
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
            Feed(Trigger.Stalking);
            entity.IsSpecialAttacking = false;
            
            FrameUtil.AfterDelay(2, () =>
            {
                if (!entity.IsDead) Feed(Trigger.Attack);
            });
        }

        private void OnTakingDamage(Damage damage)
        {
            if (damage.type == DamageType.Block && entity.CurrentAction != GroupAction.OutOfControl)
            {
                if (entity.currentShieldHealth > 0)
                {
                    entity.Animator.SetTrigger(Animations.BlockAttack);
                }
                else
                {
                    entity.Animator.SetFloat("IdleSelect", 0);
                    entity.CurrentAction = GroupAction.OutOfControl;
                }
            }
            else if(damage.type == DamageType.Back)
            {
                weakPointHit = true;
            }
        }
        
        private void OnSetAction(GroupAction newAction, GroupAction lastAction)
        {
            if (newAction == GroupAction.Stalking && entity.currentShieldHealth > 0)
            {
                Feed(Trigger.Block);
            }
            else if (newAction == GroupAction.OutOfControl)
            {
                Feed(Trigger.Attack);
            }
        }
    }
}

