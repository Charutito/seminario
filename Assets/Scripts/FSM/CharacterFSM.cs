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
            public static int SpiritPunch = 8;
            public static int GettingHitBack = 9;
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
            State<int> GettingHitBack = new State<int>("Getting Hit Back");
            State<int> SpiritPunch = new State<int>("SpiritPunch");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);


            StateConfigurer.Create(Moving)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Attacking)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(SpecialAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle);

            StateConfigurer.Create(GetHit)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(GettingHitBack)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(SpiritPunch)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);


            #region Character Events
            entity.OnAttack += FeedAttack;
            entity.OnDash += DoDash;
            entity.OnSpecialAttack += FeedSpecialAttack;
            entity.OnStun += FeedStun;
            entity.OnMove += FeedMove;
            entity.OnGetHit += FeedGetHit;
            entity.OnGettingHitBack += FeedGettingHitBack;
            entity.OnSpiritPunch += FeedSpiritPunch;


            entity.OnAttackRecovering += () => {
				entity.IsAttacking = false;
			    entity.IsSpecialAttacking = false;
			};
            entity.OnAttackRecovered += () => {
                entity.IsAttacking = false;
                entity.IsSpecialAttacking = false;
				Feed(Trigger.None);
            };
            entity.OnDeath += (e) =>
            {
                entity.OnAttack -= FeedAttack;
                entity.OnDash -= DoDash;
                entity.OnSpecialAttack -= FeedSpecialAttack;
                entity.OnStun -= FeedStun;
                entity.OnGetHit -= FeedGetHit;
                entity.OnMove -= FeedMove;
                entity.OnGettingHitBack -= FeedGettingHitBack;
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

            #region Getting Hit Back
            GettingHitBack.OnEnter += () =>
            {
                entity.Animator.SetTrigger("GetHitBack");
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;
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

            #region Spirit Punch Spell
            SpiritPunch.OnEnter += () =>
            {
                entity.OnMove -= FeedMove;
                entity.OnDash -= DoDash;
                entity.Animator.SetTrigger("SpiritPunch");
            };

            SpiritPunch.OnExit += () =>
            {
                entity.OnMove += FeedMove;
                entity.OnDash += DoDash;
            };

            #endregion

            #region Dead State
            Dead.OnEnter += () =>
            {
                entity.Animator.SetBool("Death", true);
                entity.Animator.SetTrigger("TriggerDeath");
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

        private void FeedGettingHitBack()
        {
            Feed(Trigger.GettingHitBack);
        }

        private void FeedAttack()
        {
			if (!entity.IsAttacking && !entity.IsSpecialAttacking)
			{
				Feed(Trigger.Attack);
			}
        }

        private void FeedSpiritPunch()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.SpiritPunch);
            }
        }

        private void FeedSpecialAttack()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.SpecialAttack);
            }
        }
        #endregion
    }
}

