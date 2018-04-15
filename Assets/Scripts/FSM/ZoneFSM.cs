using BattleSystem;
using Managers;
using UnityEngine;

namespace FSM
{
    public class ZoneFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int PlayerEnter = 1;
            public static int AttackTime = 2;
            public static int ZoneCleared = 3;
        }

        #region Local Vars
        private ZoneController zone;
        private float currentTimeToAttack = 0f;
        #endregion

        public ZoneFSM(ZoneController zone)
        {
            this.debugName = "ZoneFSM";
            this.zone = zone;

            #region States Definitions
            State<int> Idle = new State<int>("Idle");
            State<int> Spawning = new State<int>("Spawning");
            State<int> Attacking = new State<int>("Attacking");
            State<int> Clearing = new State<int>("Clearing");
            #endregion

            #region States Configuration
            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.PlayerEnter, Spawning);

            StateConfigurer.Create(Spawning)
                .SetTransition(Trigger.AttackTime, Attacking)
                .SetTransition(Trigger.ZoneCleared, Clearing);

            StateConfigurer.Create(Attacking)
                .SetTransition(Trigger.AttackTime, Attacking)
                .SetTransition(Trigger.ZoneCleared, Clearing);
            #endregion

            SetInitialState(Idle);

            #region Spawning State
            Spawning.OnEnter += () =>
            {
                zone.PrepareEntities();
                zone.Initialized = true;
                currentTimeToAttack = Random.Range(zone.minAttackDelay, zone.maxAttackDelay);
            };
            #endregion

            #region Attacking State
            Attacking.OnEnter += () =>
            {
                zone.ExecuteAttack();
            };
            #endregion

            #region Clearing State
            Clearing.OnEnter += () =>
            {
                
            };
            #endregion
        }

        public override void Update()
        {
            if (zone.Initialized && !GameManager.Instance.Character.IsDead)
            {
                if (currentTimeToAttack <= 0)
                {
                    Feed(Trigger.AttackTime);
                    currentTimeToAttack = Random.Range(zone.minAttackDelay, zone.maxAttackDelay);
                }

                currentTimeToAttack -= Time.deltaTime;
            }

            base.Update();
        }

        #region Zone Triggers
        public void PlayerEnter()
        {
            Feed(Trigger.PlayerEnter);
        }
        #endregion
    }
}

