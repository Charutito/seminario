using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private EntityMove _EntityMove;
        private EntityAttacker _EntityAttack;

        private bool isLocked = false;

        private int maxComboAttacks = 1;
        private int currentComboAttacks = 0;

        public CharacterFSM(Entity e)
        {
            _EntityAttack = e.gameObject.GetComponent<EntityAttacker>();
            _EntityMove = e.gameObject.GetComponent<EntityMove>();

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
                .SetTransition(CharacterInput.HeavyAttack, HeavyAttacking)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            StateConfigurer.Create(HeavyAttacking)
                .SetTransition(CharacterInput.LightAttack, HeavyAttacking)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            #region Moving
            Moving.OnUpdate += () =>
            {
                e.Stats.MoveSpeed.Current = e.Stats.MoveSpeed.Max;

                _EntityMove.MoveTransform(InputManager.Instance.AxisHorizontal, InputManager.Instance.AxisVertical);
                e.Animator.SetFloat("Velocity Z", 1);
            };

            Moving.OnExit += () =>
            {
                e.Animator.SetFloat("Velocity Z", 0);
            };
            #endregion

            #region Light Attack
            var attackDelay = 0.15f;
            var currentAttackDelay = 0f;

            LightAttacking.OnUpdate += () =>
            {
                if (InputManager.Instance.LightAttack && currentAttackDelay <= 0)
                {
                    currentAttackDelay = attackDelay;
                    isLocked = true;

                    e.Animator.SetTrigger("LightAttack");
                    _EntityAttack.LightAttack_Start();
                }
                else if (InputManager.Instance.HeavyAttack && currentComboAttacks < maxComboAttacks)
                {
                    isLocked = true;
                    currentComboAttacks++;

                    e.Animator.SetTrigger("HeavyAttack");
                    _EntityAttack.HeavyAttack_Start();
                }

                currentAttackDelay -= Time.deltaTime;
            };

            LightAttacking.OnExit += () =>
            {
                currentComboAttacks = 0;
            };
            #endregion

            #region Heavy Attack
            HeavyAttacking.OnEnter += () =>
            {
                isLocked = true;
                e.Animator.SetTrigger("HeavyAttack");
                _EntityAttack.HeavyAttack_Start();
            };
            #endregion

            e.OnThink += () =>
            {
                if (isLocked) return;

                if (InputManager.Instance.LightAttack)
                {
                    Feed(CharacterInput.LightAttack);
                }
                else if (InputManager.Instance.HeavyAttack)
                {
                    Feed(CharacterInput.HeavyAttack);
                }
                else if (InputManager.Instance.AxisMoving)
                {
                    Feed(CharacterInput.Move);
                }
                else
                {
                    Feed(CharacterInput.None);
                }
            };

            e.OnAnimUnlock += () =>
            {
                isLocked = false;
            };
        }
    }
}

