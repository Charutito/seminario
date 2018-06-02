using BattleSystem;
using Managers;
using UnityEngine;

namespace FSM
{
    public class ZoneFSM : EventFSM<int>
    {
        private class Trigger
        {
            public const int None = 0;
            public const int PlayerEnter = 1;
            public const int AttackTime = 2;
            public const int ZoneCleared = 3;
        }

        #region Local Vars
        private ZoneController _zone;
        private float _currentTimeToAction;
        #endregion

        public ZoneFSM(ZoneController zone)
        {
            this.debugName = "ZoneFSM";
            _zone = zone;
            
            _zone.OnZoneActivate.AddListener(PlayerEnter);

            #region States Definitions
            var Idle = new State<int>("Idle");
            var Spawning = new State<int>("Spawning");
            var Attacking = new State<int>("Attacking");
            var Clearing = new State<int>("Clearing");
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
                zone.Entities.ForEach(entity => entity.CurrentAction = GroupAction.Stalking);
                zone.Spawners.ForEach(spawner => spawner.Activate(zone));
                zone.Initialized = true;
                _currentTimeToAction = _zone.ActionDelay.GetRandom;
            };
            #endregion

            #region Attacking State
            Attacking.OnEnter += zone.ExecuteAttack;
            #endregion
        }

        public override void Update()
        {
            if (_zone.Initialized && !GameManager.Instance.Character.IsDead)
            {
                if (_currentTimeToAction <= 0)
                {
                    Feed(Trigger.AttackTime);
                    _currentTimeToAction = _zone.ActionDelay.GetRandom;
                }

                _currentTimeToAction -= Time.deltaTime;
            }

            base.Update();
        }

        #region Zone Triggers
        public void PlayerEnter()
        {
            _zone.OnZoneActivate.RemoveListener(PlayerEnter);
            Feed(Trigger.PlayerEnter);
        }
        #endregion
    }
}

