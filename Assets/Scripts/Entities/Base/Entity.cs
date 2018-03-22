using BattleSystem;
using UnityEngine;
using Stats;
using System;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        #region Properties
        public Animator Animator { get { return animator; } }
        public bool IsDead { get { return (stats.Health.Current <= stats.Health.Min) && !godMode; } }
        public EntityStats Stats { get { return stats; } }
        #endregion


        #region Events
        public event Action OnAnimUnlock = delegate { };
        public event Action OnAttackEnd = delegate { };
        public event Action OnThink = delegate { };
        #endregion


        #region Local Vars
        [SerializeField] private EntityStats stats;

        [SerializeField] private bool godMode = false;

        private Animator animator;
        #endregion 


        #region IDamageable
        public virtual void TakeDamage(int damage, DamageType type)
        {
            Stats.Health.Current -= damage;
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

        public void AttackEnd()
        {
            if (OnAttackEnd != null)
            {
                OnAttackEnd();
            }
        }
        #endregion

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected abstract void OnUpdate();

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