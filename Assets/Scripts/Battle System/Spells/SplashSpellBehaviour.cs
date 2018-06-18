using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Entities;
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
            public CinemachineShake.ShakeConfig Shake;
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

            if (value.Shake != null)
            {
                CinemachineShake.Instance.ShakeCamera(value.Shake.Duration, value.Shake.Amplitude, value.Shake.Frequency, ShakeNoise);
            }

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
                GameManager.Instance.Combo++;

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
