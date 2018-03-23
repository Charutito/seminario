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
        private class CharacterInput
        {
            public static int Move = 0;
            public static int LightAttack = 1;
            public static int HeavyAttack = 2;
            public static int Stun = 3;
            public static int None = 4;
        }

        private EntityMove entityMove;
        private EntityAttacker entityAttack;

        public CharacterFSM(CharacterEntity e)
        {
            entityAttack = e.gameObject.GetComponent<EntityAttacker>();
            entityMove = e.gameObject.GetComponent<EntityMove>();

            State<int> Idle = new State<int>("Idle");
            State<int> Moving = new State<int>("Moving");
            State<int> LightAttacking = new State<int>("Light Attacking");
            State<int> HeavyAttacking = new State<int>("Heavy Attacking");
            State<int> Stunned  = new State<int>("Stunned");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(CharacterInput.LightAttack, LightAttacking)
                .SetTransition(CharacterInput.HeavyAttack, HeavyAttacking)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned);

            StateConfigurer.Create(Moving)
                .SetTransition(CharacterInput.LightAttack, LightAttacking)
                .SetTransition(CharacterInput.HeavyAttack, HeavyAttacking)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            StateConfigurer.Create(LightAttacking)
                .SetTransition(CharacterInput.LightAttack, LightAttacking)
                .SetTransition(CharacterInput.HeavyAttack, HeavyAttacking)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            StateConfigurer.Create(HeavyAttacking)
                .SetTransition(CharacterInput.HeavyAttack, HeavyAttacking)
                .SetTransition(CharacterInput.LightAttack, LightAttacking)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);


            #region Character Events
            e.OnAttack += FeedAttack;
            e.OnHeavyAttack += FeedHeavyAttack;
            e.OnMove += FeedMove;

            //e.OnAttackEnd += () => { };
            e.OnAnimUnlock += () => {
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
                    entityMove.MoveTransform(InputManager.Instance.AxisHorizontal, InputManager.Instance.AxisVertical);
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
            LightAttacking.OnEnter += () =>
            {
                e.OnMove -= FeedMove;

                entityAttack.LightAttack_Start();
            };

            LightAttacking.OnExit += () =>
            {
                e.OnMove += FeedMove;
            };
            #endregion


            #region Heavy Attack
            HeavyAttacking.OnEnter += () =>
            {
                e.OnMove -= FeedMove;

                entityAttack.HeavyAttack_Start();
            };

            HeavyAttacking.OnExit += () =>
            {
                e.OnMove += FeedMove;
            };
            #endregion
        }


        #region Feed Functions
        private void FeedMove()
        {
            Feed(CharacterInput.Move);
        }

        private void FeedAttack()
        {
            Feed(CharacterInput.LightAttack);
        }

        private void FeedHeavyAttack()
        {
            Feed(CharacterInput.HeavyAttack);
        }
        #endregion
    }
}

