using Entities;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;
using Util;

namespace FSM
{
    public class CharacterFSM : EventFSM<int>
    {
        private static class Trigger
        {
            public static int Move = 0;
            public static int Attack = 1;
            public static int SpecialAttack = 2;
            public static int ChargedAttack = 3;
            public static int Stun = 4;
            public static int None = 5;
            public static int Die = 6;
            public static int GetHit = 7;
        }

        private CharacterEntity entity;

        public CharacterFSM(CharacterEntity entity)
        {
            this.debugName = "CharacterFSM";
            this.entity = entity;
            
            State<int> Idle = new State<int>("Idle");
            State<int> Moving = new State<int>("Moving");
            State<int> Dashing = new State<int>("Dash");
			State<int> Attacking = new State<int>("Light Attacking");
			State<int> SpecialAttack = new State<int>("Special Attacking");
			State<int> ChargedAttack = new State<int>("Charged Attacking");
            State<int> Stunned  = new State<int>("Stunned");
            State<int> Dead  = new State<int>("Dead");
            State<int> GetHit = new State<int>("GetHit");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Stun, Stunned);

            StateConfigurer.Create(Moving)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle);

            StateConfigurer.Create(Attacking)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle);

            StateConfigurer.Create(SpecialAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle);
            
            StateConfigurer.Create(ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle);
            
            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle);

            StateConfigurer.Create(GetHit)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle);


            #region Character Events
            entity.OnAttack += FeedAttack;
            entity.OnDash += DoDash;
            entity.OnSpecialAttack += FeedSpecialAttack;
            entity.OnChargedAttack += FeedChargedAttack;
            entity.OnStun += FeedStun;
            entity.OnMove += FeedMove;
            entity.OnShowDamage += FeedGetHit;


            entity.OnAttackRecovering += () => {
				entity.IsAttacking = false;
			    entity.IsSpecialAttacking = false;
			};
            entity.OnAttackRecovered += () => {
				Feed(Trigger.None);
            };
            entity.OnDeath += (e) =>
            {
                entity.OnAttack -= FeedAttack;
                entity.OnDash -= DoDash;
                entity.OnSpecialAttack -= FeedSpecialAttack;
                entity.OnChargedAttack -= FeedChargedAttack;
                entity.OnStun -= FeedStun;
                entity.OnMove -= FeedMove;
                Feed(Trigger.Die);
            };
            #endregion


            #region Moving
            Moving.OnEnter += () =>
            {
                entity.Stats.MoveSpeed.Current = entity.Stats.MoveSpeed.Max;
                entity.Animator.SetFloat("Velocity Z", 1);
                entity.OnDash += DoDash;
            };

            Moving.OnUpdate += () =>
            {
                if (InputManager.Instance.AxisMoving)
                {
                    entity.EntityMove.MoveTransform(InputManager.Instance.AxisHorizontal, InputManager.Instance.AxisVertical);
                }
                else
                {
                    Feed(Trigger.None);
                }
            };

            Moving.OnExit += () =>
            {
                entity.Animator.SetFloat("Velocity Z", 0);
                entity.OnDash -= DoDash;
            };
            #endregion

            #region Light Attack
            Attacking.OnEnter += () =>
            {
				entity.IsAttacking = true;

                entity.OnMove -= FeedMove;
                entity.EntityAttacker.LightAttack_Start();
            };

            Attacking.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion


            #region Special Attack
            SpecialAttack.OnEnter += () =>
            {
                entity.IsSpecialAttacking = true;

                entity.OnMove -= FeedMove;
                entity.EntityAttacker.HeavyAttack_Start();
            };

            SpecialAttack.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion
            
            
            #region Charged Attack
            var pusheen = 0f;
            var maxPusheen = 3f;
            var isCharging = false;
            
            ChargedAttack.OnEnter += () =>
            {
                isCharging = true;
                entity.IsSpecialAttacking = true;

                entity.OnMove -= FeedMove;
                entity.Animator.SetTrigger("ChargedAttack");
            };
            
            ChargedAttack.OnUpdate += () =>
            {
                if (isCharging)
                {
                    pusheen += Time.deltaTime;

                    if (InputManager.Instance.ChargedAttackUp || pusheen >= maxPusheen)
                    {
                        isCharging = false;
                        entity.Animator.SetTrigger("ChargedAttackStart");
                    }
                }
            };

            ChargedAttack.OnExit += () =>
            {
                pusheen = 0;
                entity.IsSpecialAttacking = false;
                entity.OnMove += FeedMove;
            };
            #endregion
            
            #region Stunned State
            Stunned.OnEnter += () =>
            {
                entity.Animator.SetTrigger("Countered");
            };
            #endregion

            #region GetHit State
            GetHit.OnEnter += () =>
            {
                entity.Animator.SetTrigger("GetHit");
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;
            };
            #endregion

            #region Dead State
            Dead.OnEnter += () =>
            {
                entity.Animator.SetTrigger("Death");
            };
            #endregion
        }

        private void DoDash()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking && entity.currentDashCharges > 0)
            {
                var dashPosition = entity.transform.position +
                                   entity.EntityAttacker.lineArea.transform.forward * entity.dashLenght;
                
                entity.currentDashCharges--;
                entity.EntityMove.RotateInstant(dashPosition);
                entity.EntityMove.SmoothMoveTransform(dashPosition, 0.1f);
            }
        }

        #region Feed Functions
        private void FeedStun()
        {
            Feed(Trigger.Stun);
        }
        
        private void FeedMove()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.Move);
            }
        }

        private void FeedGetHit()
        {
            Feed(Trigger.GetHit);
        }

        private void FeedAttack()
        {
			if (!entity.IsAttacking && !entity.IsSpecialAttacking)
			{
				Feed(Trigger.Attack);
			}
        }

        private void FeedSpecialAttack()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.SpecialAttack);
            }
        }
        
        private void FeedChargedAttack()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.ChargedAttack);
            }
        }
        #endregion
    }
}

