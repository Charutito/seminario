using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "Akane/Entities/Definition")]
    public sealed class EntityDefinition : ScriptableObject
    {
        #region Events

        public event Action<int, int> OnHealthChange = delegate {}; 
        public event Action<int, int> OnSpiritChange = delegate {}; 

        #endregion
        
        #region Properties
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                if (_currentHealth != value)
                {
                    OnHealthChange(_currentHealth, value);
                    _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                }
            }
        }
        
        public int CurrentSpirit
        {
            get { return _currentSpirit; }
            set
            {
                if (_currentSpirit != value)
                {
                    OnSpiritChange(_currentSpirit, value);
                    _currentSpirit = Mathf.Clamp(value, 0, MaxSpirit);
                }
            }
        }
        #endregion
        
        
        public string Name = string.Empty;
        
        [Header("Stats")]
        public int MaxHealth = 100;
        public int MaxSpirit = 100;
        
        [Header("Movenment")]
        public float MovementSpeed = 10;
        public float RotationSpeed = 10;

        [Header("Damage")]
        public int LightAttackDamage = 10;
        public int SpecialAttackDamage = 25;
        
        [Header("Spirit Recovery")]
        public int LightRecovery = 2;
        public int HeavyRecovery = 5;
        
        [Header("Spawn")]
        public GameObject Prefab;


        private int _currentHealth;
        private int _currentSpirit;

        public EntityDefinition CreateInstance()
        {
            var newInstance = Instantiate(this);

            newInstance.CurrentHealth = newInstance.MaxHealth;
            newInstance.CurrentSpirit = newInstance.MaxSpirit;

            return newInstance;
        }
    }
}
