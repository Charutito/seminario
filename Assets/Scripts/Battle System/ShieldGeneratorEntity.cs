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
                                        .Where(e => e != this)
                                        .Where(e => _activeShields.All(activeEntity => activeEntity.Entity != e))
                                        .OrderBy(e => Random.value)
                                        .FirstOrDefault();

            if (entity != null)
            {
                var newObject = Instantiate(ShieldPrefab, entity.transform.position, Quaternion.identity, entity.transform);
                
                _activeShields.Add(newObject.GetComponent<EnemyShield>());
                EntitySounds.PlayEffect("Cast", entity.transform.position);
            }

            _currentTimeToCast = CastDelay.GetRandom;
        }

        private void HandleDeath(Entity obj)
        {
            EntitySounds.PlayEffect("Destroy");
            _active = false;
            foreach (var activeShield in _activeShields)
            {
                if (activeShield != null)
                {
                    Destroy(activeShield.gameObject);
                }
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
