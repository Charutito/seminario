using BattleSystem;
using Managers;
using UnityEngine;
using Util;

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
            #endregion

            #region States Configuration
            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.PlayerEnter, Spawning);

            StateConfigurer.Create(Spawning)
                .SetTransition(Trigger.AttackTime, Attacking);
            #endregion

            SetInitialState(Idle);

            #region Spawning State
            Spawning.OnEnter += () =>
            {
                zone.Initialized = true;
                zone.Entities.ForEach(entity => entity.CurrentAction = GroupAction.Stalking);
                zone.Spawners.ForEach(spawner => spawner.Activate(zone));
                
                FrameUtil.OnNextFrame(() => Feed(Trigger.AttackTime));
            };
            #endregion

            #region Attacking State
            Attacking.OnUpdate += () =>
            {
                if (GameManager.Instance.Character.IsDead) return;

                _currentTimeToAction -= Time.deltaTime;
                
                if (_currentTimeToAction <= 0)
                {
                    zone.ExecuteAttack();
                    _currentTimeToAction = _zone.ActionDelay.GetRandom;
                }
            };
            #endregion
        }

        #region Zone Triggers
        public void PlayerEnter(ZoneController zone)
        {
            zone.OnZoneActivate.RemoveListener(PlayerEnter);
            Feed(Trigger.PlayerEnter);
        }
        #endregion
    }
}

