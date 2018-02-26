using BattleSystem;
using UnityEngine;
using Stats;
using System;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    public abstract class Entity : MonoBehaviour, ITargettable, IDamageable
    {
        #region Properties
        public Animator Animator { get { return _animator; } }
        public bool IsDead { get { return (_stats.Health.Actual <= _stats.Health.Min) && !_godMode; } }
        public EntityStats Stats { get { return _stats; } }
        #endregion


        #region Events
        public event Action OnAnimUnlock = delegate { };
        public event Action OnThink = delegate { };
        #endregion


        #region Local Vars
        [SerializeField]
        private EntityStats _stats;

        [SerializeField]
        private bool _godMode = false;

        private Animator _animator;
        #endregion 


        #region Interfaces
        public virtual void TakeDamage(int damage)
        {
            Stats.Health.Actual -= damage;
        }

        public virtual Transform Target()
        {
            return transform;
        }
        #endregion


        #region Animation Events
        public void AnimUnlock()
        {
            if (OnAnimUnlock != null)
            {
                OnAnimUnlock();
            }
        }
        #endregion

        protected abstract void OnUpdate();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Por el momento esto esta bien, pero tendria uqe estar en una corrutina
            // o en un UpdateManager para poder frenar el pensamiento de la FSM
            if (OnThink != null)
            {
                OnThink();
            }

            OnUpdate();
        }
    }
}