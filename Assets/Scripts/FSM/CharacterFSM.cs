using Entities;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace FSM
{
    public class CharacterFSM : EventFSM<int>
    {
        private static class CharacterInput
        {
            public static int Move = 0;
            public static int Attack = 1;
            public static int SpecialAttack = 2;
            public static int ChargedAttack = 3;
            public static int Stun = 4;
            public static int None = 5;
        }

		private bool isAttacking = false;

        public CharacterFSM(CharacterEntity e)
        {
            State<int> Idle = new State<int>("Idle");
            State<int> Moving = new State<int>("Moving");
			State<int> Attacking = new State<int>("Light Attacking");
			State<int> SpecialAttack = new State<int>("Special Attacking");
			State<int> ChargedAttack = new State<int>("Charged Attacking");
            State<int> Stunned  = new State<int>("Stunned");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(CharacterInput.Attack, Attacking)
                .SetTransition(CharacterInput.SpecialAttack, SpecialAttack)
                .SetTransition(CharacterInput.ChargedAttack, ChargedAttack)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned);

            StateConfigurer.Create(Moving)
                .SetTransition(CharacterInput.Attack, Attacking)
                .SetTransition(CharacterInput.SpecialAttack, SpecialAttack)
                .SetTransition(CharacterInput.ChargedAttack, ChargedAttack)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            StateConfigurer.Create(Attacking)
                .SetTransition(CharacterInput.Attack, Attacking)
                .SetTransition(CharacterInput.SpecialAttack, SpecialAttack)
                //.SetTransition(CharacterInput.ChargedAttack, ChargedAttack)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            StateConfigurer.Create(SpecialAttack)
                /*.SetTransition(CharacterInput.Attack, Attacking)
                .SetTransition(CharacterInput.ChargedAttack, ChargedAttack)
                .SetTransition(CharacterInput.Move, Moving)*/
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);
            
            StateConfigurer.Create(ChargedAttack)
                /*.SetTransition(CharacterInput.Attack, Attacking)
                .SetTransition(CharacterInput.SpecialAttack, SpecialAttack)
                .SetTransition(CharacterInput.Move, Moving)*/
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);


            #region Character Events
            e.OnAttack += FeedAttack;
            e.OnSpecialAttack += FeedSpecialAttack;
            e.OnChargedAttack += FeedChargedAttack;
            e.OnMove += FeedMove;
			e.OnAttackRecovering += () => {
				isAttacking = false;
			};
            e.OnAttackRecovered += () => {
				Feed(CharacterInput.None);
            };

            #endregion


            #region Moving
            Moving.OnEnter += () =>
            {
                e.Stats.MoveSpeed.Current = e.Stats.MoveSpeed.Max;
                e.Animator.SetFloat("Velocity Z", 1);
            };

            Moving.OnUpdate += () =>
            {
                if (InputManager.Instance.AxisMoving)
                {
                    e.EntityMove.MoveTransform(InputManager.Instance.AxisHorizontal, InputManager.Instance.AxisVertical);
                }
                else
                {
                    Feed(CharacterInput.None);
                }
            };

            Moving.OnExit += () =>
            {
                e.Animator.SetFloat("Velocity Z", 0);
            };
            #endregion


            #region Light Attack
            Attacking.OnEnter += () =>
            {
				isAttacking = true;

                e.OnMove -= FeedMove;
                e.EntityAttacker.LightAttack_Start();
            };

            Attacking.OnExit += () =>
            {
                e.OnMove += FeedMove;
            };
            #endregion


            #region Special Attack
            SpecialAttack.OnEnter += () =>
            {
                isAttacking = true;

                e.OnMove -= FeedMove;
                e.EntityAttacker.HeavyAttack_Start();
            };

            SpecialAttack.OnExit += () =>
            {
                e.OnMove += FeedMove;
            };
            #endregion
            
            
            #region Charged Attack
            var pusheen = 0f;
            var minPusheen = 0.5f;
            var maxPusheen = 3f;
            var isCharging = false;
            
            ChargedAttack.OnEnter += () =>
            {
                isCharging = true;
                isAttacking = true;

                e.OnMove -= FeedMove;
                e.Animator.SetTrigger("ChargedAttack");
            };
            
            ChargedAttack.OnUpdate += () =>
            {
                if (isCharging)
                {
                    pusheen += Time.deltaTime;

                    if (InputManager.Instance.ChargedAttackUp || pusheen >= maxPusheen)
                    {
                        isCharging = false;
                        e.Animator.SetTrigger("ChargedAttackStart");
                    }
                }
            };

            ChargedAttack.OnExit += () =>
            {
                pusheen = 0;
                isAttacking = false;
                e.OnMove += FeedMove;
            };
            #endregion
        }


        #region Feed Functions
        private void FeedMove()
        {
			if(!isAttacking)
            Feed(CharacterInput.Move);
        }

        private void FeedAttack()
        {
			if (!isAttacking)
			{
				Feed(CharacterInput.Attack);
			}
        }

        private void FeedSpecialAttack()
        {
            if (!isAttacking)
            {
                Feed(CharacterInput.SpecialAttack);
            }
        }
        
        private void FeedChargedAttack()
        {
            if (!isAttacking)
            {
                Feed(CharacterInput.ChargedAttack);
            }
        }
        #endregion
    }
}

