using BattleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Stats;

namespace Entities
{
    public class Entity : MonoBehaviour, ITargettable, IDamageable
    {
        #region Properties
        public bool IsDead { get { return (_stats.Health.Actual <= _stats.Health.Min) && !_godMode; } }
        public EntityStats Stats { get { return _stats; } }
        #endregion

        #region Local Vars
        [SerializeField] private EntityStats _stats;

        [SerializeField] private bool _godMode = false;
        #endregion

        #region Interfaces
        public virtual void Hit(int damage)
        {
            Stats.Health.Actual -= damage;
        }

        public virtual Transform Target()
        {
            return transform;
        }
        #endregion
    }
}