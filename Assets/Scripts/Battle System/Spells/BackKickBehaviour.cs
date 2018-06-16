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
        public int LifeRecover = 2;
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
            var part = Instantiate(_behaviour.Definition.SubCast,pos);
            part.transform.position = new Vector3(pos.position.x,pos.position.y+1,pos.position.z);
            part.transform.SetParent(pos);
        }
        
        private void Cast()
        {            
            var enemies = _lineOfAim.GetEnemiesInSight().ToList();
            foreach (var enemy in enemies)
            {
                DmgCast(enemy.transform);
                GameManager.Instance.Combo++;
                _character.Heal(LifeRecover);
                enemy.TakeDamage(new Damage
                {
                    Amount = _behaviour.Definition.Damage,
                    Type = _behaviour.Definition.DamageType,
                    Origin = transform,
                    Originator = _character,
                    Displacement = Displacement
                });
            }
        }
    }
}
