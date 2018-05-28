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
        private void DmgCast(Transform pos)
        {
            var part = Instantiate(_behaviour.Definition.HitEffect);
            var partpos = new Vector3(pos.position.x, pos.position.y + 1, pos.position.z);
            part.transform.position = partpos;
        }
        
        private void Cast()
        {
            var part = Instantiate(_behaviour.Definition.SubCast);
            part.transform.position = transform.position;
            part.transform.forward = GameManager.Instance.Character.transform.forward;
            var enemies = _lineOfAim.GetEnemiesInSight().ToList();
            foreach (var enemy in enemies)
            {
                GameManager.Instance.Combo++;
                _character.Heal(LifeRecover);
                DmgCast(enemy.transform);
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
