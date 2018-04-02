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
        }
        private class Animations
        {
            public static string Attack = "Attack";
            public static string SpecialAttack = "SpecialAttack";
            public static string Death = "Death";
            public static string Aim = "Aim";
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
                .SetTransition(Trigger.Aim, aim)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(aim)
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

            #region Aim State
            aim.OnUpdate += () =>
            {
                //setear animator en "aim"
                entity.EntityMove.RotateTowards(entity.Target.transform.position);
            };
            #endregion

        }

    }
    }