using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Managers;
using UnityEngine;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
    public class DancingBladesSpell : MonoBehaviour
    {
        public float DeltaSlow = 0.5f;
        private SpellBehaviour _behaviour;
        private bool _previousStatus;
        private CharacterEntity _character;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
            _character = GameManager.Instance.Character;
            
            Cast();
        }

        private void Cast()
        {
            _previousStatus = _character.IsInvulnerable;
            _character.IsInvulnerable = true;
            
            if (_character.EntityAttacker.lineArea != null)
            {
                var enemies = _character.EntityAttacker.lineArea.GetEnemiesInSight()
                    .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
                    .Take(_behaviour.Definition.MaxAffected)
                    .ToList();
                
                StartCoroutine(PotatoFest(enemies));
            }
        }
        
        private IEnumerator PotatoFest(IEnumerable<Entity> targets)
        {
            Time.timeScale = DeltaSlow;
            var halfWait = _behaviour.Definition.WaitTime / 2;

            var casted = false;
            
            foreach (var enemy in targets)
            {
                casted = true;
                _character.EntityMove.RotateInstant(enemy.transform.position);
                _character.EntityMove.SmoothMoveTransform(enemy.transform.position - transform.forward, halfWait);
                
                enemy.TakeDamage(new Damage
                {
                    amount = _behaviour.Definition.Damage,
                    type = _behaviour.Definition.DamageType,
                    origin = transform,
                    originator = _character
                });
                
                yield return new WaitForSeconds(_behaviour.Definition.WaitTime);
            }

            if (casted)
            {
                _character.EntityMove.SmoothMoveTransform(_character.transform.position + _character.transform.forward * 2, halfWait);
            }
            
            Time.timeScale = 1f;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _character.IsInvulnerable = _previousStatus;
        }
    }
}
