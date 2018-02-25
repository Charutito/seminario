using Entities;
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
            public static int Attack = 1;
            public static int Stun = 2;
            public static int None = 3;
        }

        private EntityMove _EntityMove;
        private EntityAttacker _EntityAttack;

        private bool isLocked = false;
        private int 

        public CharacterFSM(Entity e)
        {
            _EntityAttack = e.gameObject.GetComponent<EntityAttacker>();
            _EntityMove = e.gameObject.GetComponent<EntityMove>();

            State<int> Idle = new State<int>("Idle");
            State<int> Moving = new State<int>("Moving");
            State<int> Attack  = new State<int>("Attacking");
            State<int> Stunned  = new State<int>("Stunned");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(CharacterInput.Attack, Attack)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned);

            StateConfigurer.Create(Moving)
                .SetTransition(CharacterInput.Attack, Attack)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            StateConfigurer.Create(Attack)
                .SetTransition(CharacterInput.Move, Moving)
                .SetTransition(CharacterInput.Stun, Stunned)
                .SetTransition(CharacterInput.None, Idle);

            Moving.OnUpdate += () =>
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    e.Stats.MoveSpeed.Actual = e.Stats.MoveSpeed.Max;
                }
                else
                {
                    e.Stats.MoveSpeed.Actual = e.Stats.MoveSpeed.Min;
                }

                _EntityMove.MoveTransform(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                e.Animator.SetFloat("Velocity Z", 1);
            };

            Moving.OnExit += () =>
            {
                e.Animator.SetFloat("Velocity Z", 0);
            };

            Attack.OnEnter += () =>
            {
                e.Animator.SetTrigger("Attack");
            };

            Attack.OnUpdate += () =>
            {
                if (Input.GetMouseButtonDown(0))
                {
                    e.Animator.SetTrigger("Attack");
                }
            };

            e.OnThink += () =>
            {
                if (isLocked) return;

                if (Input.GetMouseButtonDown(0))
                {
                    Feed(CharacterInput.Attack);
                }
                else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                {
                    Feed(CharacterInput.Move);
                }
                else
                {
                    Feed(CharacterInput.None);
                }
            };
        }

        private IEnumerator UnlockCd()
        {
            isLocked = true;
            yield return new WaitForSeconds(1);
            isLocked = false;
        }
    }
}

