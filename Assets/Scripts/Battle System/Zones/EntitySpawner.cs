using System;
using Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;

namespace BattleSystem
{
    public class EntitySpawner : MonoBehaviour
    {
        [Header("Configuration")]
        public EntityDefinition EntityDefinition;
        
        [Header("Spawn")]
        [MinMaxRange(0, 10)]
        public RangedFloat SpawnDelay;
        public int UnitsToSpawn = 5;
        
        [Header("Events (in execution order)")]
        public SpawnerEvent OnSpawnerSetup;
        public SpawnerEvent OnSpawnerActivate;
        public SpawnerEvent OnSpawnerCleared;

        private Image _cooldownImage;

        private int _currentSpawnedUnits;
        private float _currentTimeToSpawn;
        private float _lastSpawnDelay = 1f;
        private bool _active = false;
        private ZoneController _zone;

        private void Start()
        {
            _cooldownImage = GetComponentInChildren<Image>();
            
            OnSpawnerSetup.Invoke(this);
        }

        public void Activate(ZoneController zone)
        {
            _active = true;
            _zone = zone;
            OnSpawnerActivate.Invoke(this);
        }
        
        public void Deactivate()
        {
            _active = false;
            OnSpawnerCleared.Invoke(this);
        }

        private void Spawn()
        {
            var go = Instantiate(EntityDefinition.Prefab, transform.position + Vector3.up, transform.rotation, _zone.transform);
            var newEntity = go.GetComponent<Entity>();
            newEntity.Definition = EntityDefinition;
            
            FrameUtil.OnNextFrame(() => _zone.AddEntity(newEntity as GroupEntity, GroupAction.Attacking));
            
            _lastSpawnDelay = SpawnDelay.GetRandom;
            _currentTimeToSpawn = _lastSpawnDelay;
            _currentSpawnedUnits++;
        }

        private void Update()
        {
            if (!_active) return;
            
            if (_currentSpawnedUnits < UnitsToSpawn)
            {
                _currentTimeToSpawn -= Time.deltaTime;

                if (_cooldownImage != null)
                {
                    _cooldownImage.fillAmount = 1 - _currentTimeToSpawn / _lastSpawnDelay;
                }

                if (_currentTimeToSpawn <= 0)
                {
                    Spawn();
                }
            }
            else
            {
                Deactivate();
            }
        }
    }

    [Serializable]
    public sealed class SpawnerEvent : UnityEvent<EntitySpawner> {}
}
