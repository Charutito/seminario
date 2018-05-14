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
    public class BackKickBehaviour : MonoBehaviour
    {
        public float Displacement = 1f;
        public float LifeRecover = 2f;
        private SpellBehaviour _behaviour;
        private CharacterEntity _character;
        private LineOfAim _lineOfAim;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
            _character = GameManager.Instance.Character;
            _lineOfAim = GetComponentInChildren<LineOfAim>();
            
            Cast();
        }

        private void Cast()
        {
            var enemies = _lineOfAim.GetEnemiesInSight().ToList();
            
            foreach (var enemy in enemies)
            {
                GameManager.Instance.Combo++;
                _character.Heal(LifeRecover);

                enemy.TakeDamage(new Damage
                {
                    amount = _behaviour.Definition.Damage,
                    type = _behaviour.Definition.DamageType,
                    origin = transform,
                    originator = _character,
                    Displacement = Displacement
                });
            }
        }
    }
}
