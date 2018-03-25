using BattleSystem;
using Entities;
using UnityEngine;

namespace FSM
{
    public class BasicEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int Stalking = 2;
        }

        /// <summary>
        /// Variables and triggers in animator
        /// </summary>
        private class Animations
        {
            public static string Attack         = "LightAttack";
            public static string SpecialAttack  = "HeavyAttack";
            public static string Death          = "Death";
            public static string RandomDeath    = "RandomDeath";
            public static string Move           = "Velocity Z";
        }

        #region Components
        private EntityMove entityMove;
        private EntityAttacker entityAttack;
        #endregion

        #region Local Vars
        private string attackAnimation = string.Empty;
        private bool isLocked = false;
        #endregion

        public BasicEnemyFSM(BasicEnemy e)
        {
            this.debugName = "BasicFSM";

            entityAttack = e.gameObject.GetComponent<EntityAttacker>();
            entityMove = e.gameObject.GetComponent<EntityMove>();

            #region States Definitions
            State<int> Idle = new State<int>("Idling");
            State<int> Stalk = new State<int>("Stalking");
            State<int> Follow = new State<int>("Following");
            State<int> Attack = new State<int>("Attacking");
            #endregion


            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Stalking, Stalk);

            StateConfigurer.Create(Stalk)
                .SetTransition(Trigger.Attack, Follow);

            StateConfigurer.Create(Follow)
                .SetTransition(Trigger.Attack, Attack);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Stalking, Stalk);
            #endregion

            #region Stalk State
            Stalk.OnUpdate += () =>
            {
                entityMove.RotateTowards(e.Target.transform.position);
            };
            #endregion

            #region Follow State
            Follow.OnEnter += () =>
            {
                e.Animator.SetFloat(Animations.Move, 1);
            };

            Follow.OnUpdate += () =>
            {
                entityMove.RotateInstant(e.Target.transform.position);
                entityMove.MoveAgent(e.Target.transform.position);

                if (Vector3.Distance(e.transform.position, e.Target.transform.position) <= e.AttackRange)
                {
                    Feed(Trigger.Attack);
                }
            };

            Follow.OnExit += () =>
            {
                e.Animator.SetFloat(Animations.Move, 0);
            };
            #endregion


            #region Attack State
            Attack.OnEnter += () =>
            {
                e.Animator.SetTrigger(attackAnimation);
            };
            #endregion

            e.OnAnimUnlock += () =>
            {
                e.CurrentAction = GroupAction.Stalking;
            };

            e.OnSetAction += (GroupAction newAction, GroupAction lastAction) =>
            {
                if (newAction == GroupAction.Attacking)
                {
                    attackAnimation = Animations.Attack;
                    Feed(Trigger.Attack);
                }
                else if (newAction == GroupAction.SpecialAttack)
                {
                    attackAnimation = Animations.SpecialAttack;
                    Feed(Trigger.Attack);
                }
                else if (newAction == GroupAction.Stalking)
                {
                    Feed(Trigger.Stalking);
                }
            };
        }
    }
}

