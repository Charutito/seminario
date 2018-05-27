using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
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
        private List<GameObject> _mesh;
        private GameObject _white;
        private LineOfAim _lineOfAim;
        private CinemachineFramingTransposer _camera;
        private float _previousLookAheadTime;
        private float _previousLookAheadSmooth;
        
        private void Start()
        {
            _behaviour = GetComponent<SpellBehaviour>();
            _character = GameManager.Instance.Character;
            _lineOfAim = GetComponentInChildren<LineOfAim>();
            _mesh = GameObject.FindGameObjectsWithTag("Body").ToList();

            _camera = GameObject.FindGameObjectWithTag("GameCamera")
                .GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineFramingTransposer>();
            
            Cast();
        }

        private void Cast()
        {
            _previousStatus = _character.IsInvulnerable;
            _character.IsInvulnerable = true;

            foreach (var meshito in _mesh)
            {
                meshito.SetActive(false);
            }
            
            var enemies = _lineOfAim.GetEnemiesInSight()
                .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
                .Where(e => _character.EntityMove.CanReachPosition(e.transform.position))
                .Take(_behaviour.Definition.MaxAffected)
                .ToList();
            
            _previousLookAheadTime = _camera.m_LookaheadTime;
            _previousLookAheadSmooth = _camera.m_LookaheadSmoothing;
            
            _camera.m_LookaheadTime = 0;
            _camera.m_LookaheadSmoothing = 0;
            
            StartCoroutine(PotatoFest(enemies));
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
                
                GameManager.Instance.Combo++;
                DmgCast(enemy.transform);
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

            _character.AttackRecovered();
            Destroy(gameObject);
        }
        private void DmgCast(Transform pos)
        {
            var part = Instantiate(_behaviour.Definition.SubCast);
            var partpos = new Vector3(pos.position.x, pos.position.y + 1, pos.position.z);
            part.transform.position = partpos;
            Destroy(part, 1.5f);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            _camera.m_LookaheadTime = _previousLookAheadTime;
            _camera.m_LookaheadSmoothing = _previousLookAheadSmooth;
            
            _character.IsInvulnerable = _previousStatus;
            
            foreach (var meshito in _mesh)
            {
                meshito.SetActive(true);
            }
        }
    }
}
