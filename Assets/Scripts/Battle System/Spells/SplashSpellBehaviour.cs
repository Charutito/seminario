using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Managers;
using UnityEngine;
using Util;

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
            public List<GameObject> Objects;
        }

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
                    amount = _behaviour.Definition.Damage,
                    type = _behaviour.Definition.DamageType,
                    origin = transform,
                    originator = _character,
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
