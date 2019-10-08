using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Entities;
using EZCameraShake;
using Managers;
using UnityEngine;
using Util;
using Utility;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
    public class SplashSpellBehaviour : MonoBehaviour
    {
        [Serializable]
        public class SplashValues
        {
            public float Delay = 0.5f;
            public float Range = 2f;
            public float DeltaTime = 0.7f;
            public List<AudioEvent> Sounds;
            public List<GameObject> Objects;
        }

        public NoiseSettings ShakeNoise;

        public List<SplashValues> Values;
        
        private SpellBehaviour _behaviour;
        private bool _previousStatus;
        private CharacterEntity _character;
        private LineOfAim _lineOfAim;
        private float _maxDelay;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
            _character = GameManager.Instance.Character;
            _lineOfAim = GetComponentInChildren<LineOfAim>();

            _previousStatus = _character.IsInvulnerable;
            _character.IsInvulnerable = true;
            foreach (var value in Values)
            {
                _maxDelay += value.Delay;
                var tmpValue = value;
                FrameUtil.AfterDelay(value.Delay, () => Cast(tmpValue));
            }
            
            FrameUtil.AfterDelay(_maxDelay, () => Destroy(gameObject));
        }

        private void Cast(SplashValues value)
        {
            Time.timeScale = value.DeltaTime;

            InputManager.Instance.Vibrate(0.7f, 0.3f, 0.2f);
            CameraShaker.Instance.ShakeOnce(5f, 0.5f, 0.1f, 0.5f);

            foreach (var sound in value.Sounds)
            {
                sound.PlayAtPoint(transform.position);
            }
            
            var enemies = _lineOfAim.GetEnemiesInSight()
                .Where(e => Vector3.Distance(transform.position, e.transform.position) <= value.Range)
                .ToList();

            foreach (var go in value.Objects)
            {
                go.SetActive(true);
            }
            
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(new Damage
                {
                    Amount = _behaviour.Definition.Damage,
                    Type = _behaviour.Definition.DamageType,
                    OriginPosition = transform.position,
                    OriginRotation = transform.rotation,
                    Displacement = 0.5f,
                    Absolute = true
                });
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            _character.IsInvulnerable = _previousStatus;
        }
    }
}
