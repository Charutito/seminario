using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace BattleSystem
{
    public class EntitySpawner : MonoBehaviour
    {
        public event Action<EntitySpawner> OnComplete = delegate {};
        
        public EntityDefinition EntityDefinition;
        [MinMaxRange(0, 10)]
        public RangedFloat SpawnDelay;
        public int UnitsToSpawn = 5;
    
        [SerializeField]
        private Image _cooldownImage;

        private int _currentSpawnedUnits;
        private float _currentTimeToSpawn;
        private float _lastSpawnDelay = 1f;
        private bool _active = false;
        private ZoneController _zone;

        public void Activate(ZoneController zone)
        {
            _active = true;
            _zone = zone;
        }

        private void Spawn()
        {
            var newOjb = Instantiate(EntityDefinition.Prefab, transform.position + Vector3.up, transform.rotation, _zone.transform);
            var newEntity = newOjb.GetComponent<Entity>();
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

                _cooldownImage.fillAmount = 1 - _currentTimeToSpawn / _lastSpawnDelay;

                if (_currentTimeToSpawn <= 0)
                {
                    Spawn();
                }
            }
            else
            {
                _active = false;
                OnComplete(this);
            }
        }
    }
}
