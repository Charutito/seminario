using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using Entities;
using UnityEngine;

namespace Entities
{
    public class ShieldGeneratorEntity : GroupEntity
    {
        [MinMaxRange(0, 10)]
        public RangedFloat CastDelay;

        public GameObject ShieldPrefab;

        private bool _active;
        private float _currentTimeToCast;
        private List<EnemyShield> _activeShields;
        
        private void SetupShield()
        {
            var entity = CurrentZone.entities
                                        .Where(x => x != this)
                                        .OrderBy(x => Random.value)
                                        .FirstOrDefault();

            if (entity != null)
            {
                var newObject = Instantiate(ShieldPrefab, entity.transform.position, Quaternion.identity, entity.transform);
                
                _activeShields.Add(newObject.GetComponent<EnemyShield>());
            }

            _currentTimeToCast = CastDelay.GetRandom;
        }

        private void HandleDeath(Entity obj)
        {
            _active = false;
            foreach (var activeShield in _activeShields)
            {
                Destroy(activeShield.gameObject);
            }
            Destroy(gameObject, 1f);
        }

        protected override void Update()
        {
            if (_active)
            {
                if (_currentTimeToCast <= 0)
                {
                    SetupShield();
                }
                
                _currentTimeToCast -= Time.deltaTime;
            }

            base.Update();
        }
        
        protected override void SetFsm()
        {
            _activeShields = new List<EnemyShield>();
            OnDeath += HandleDeath;
            OnSetAction += (action, groupAction) => _active = true;
        }
    }
}
