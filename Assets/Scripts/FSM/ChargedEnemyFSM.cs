using System.Runtime.InteropServices;
using BattleSystem;
using Entities;
using UnityEngine;
using Util;

namespace FSM
{
    public class ChargedEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int Die = 2;
            public static int Recover = 3;
            public static int Charging = 4;
        }

        private class Animations
        {
            public static string Attack = "Attack";
            public static string Recovering = "Recovering";
            public static string ChargeAttack = "ChargeAttack";
            public static string Death = "Death";
            public static string RandomDeath = "RandomDeath";
            public static string Move = "Velocity Z";
            public static string Charging = "Charging";
        }

        #region Components
        private ChargedEnemy entity;
        #endregion


        public ChargedEnemyFSM(ChargedEnemy entity)
        {
            this.debugName = "ChargedEnemyFSM";
            this.entity = entity;

            #region States Definitions
            State<int> Idle = new State<int>("Idling");
            State<int> Charge = new State<int>("Charging");
            State<int> Attack = new State<int>("Attacking");
            State<int> Death = new State<int>("Death");
            State<int> Recovering = new State<int>("Recovering");
            #endregion


            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Charging, Charge)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Charge)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Recover, Recovering)
                .SetTransition(Trigger.Die, Death);

            StateConfigurer.Create(Recovering)
                .SetTransition(Trigger.Charging, Charge)
                .SetTransition(Trigger.Die, Death);
            #endregion

            #region Charge State
            Charge.OnUpdate += () =>
            {
                
            };
            #endregion



        }



    }
    }
