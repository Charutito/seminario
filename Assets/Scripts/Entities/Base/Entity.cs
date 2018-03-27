using BattleSystem;
using UnityEngine;
using Stats;
using System;
using UnityEngine.AI;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        #region EntityComponents
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; }
        public Collider Collider { get; private set; }
        public EntityAttacker EntityAttacker { get; private set; }
        public EntityMove EntityMove { get; private set; }
        #endregion
        
        #region Stats
        public bool IsDead { get { return stats.Health.Current <= stats.Health.Min; } }
        public bool IsGod { get { return godMode; } }
        public EntityStats Stats { get { return stats; } }
        #endregion


        #region Events
        public event Action<int, DamageType> OnTakeDamage = delegate { };
        public event Action<Entity> OnDeath = delegate { };
        #endregion


        #region Local Vars
        [SerializeField] private EntityStats stats;
        [SerializeField] private bool godMode = false;
        #endregion 


        #region IDamageable
        public virtual void TakeDamage(int damage, DamageType type)
        {
            if (IsDead) return;

            if (!godMode)
            {
                Stats.Health.Current -= damage;
            }

            if (IsDead && OnDeath != null)
            {
                OnDeath(this);
            }

            if (OnTakeDamage != null)
            {
                OnTakeDamage(damage, type);
            }
        }
        #endregion

        protected virtual void Awake()
        {
            Stats.Health.Current = stats.Health.Max;
            
            TryGetComponents();
        }

        private void TryGetComponents()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Collider = GetComponent<Collider>();
            EntityAttacker = GetComponent<EntityAttacker>();
            EntityMove = GetComponent<EntityMove>();
        }
        
        // Animator.SetFloat("Velocity Z", Vector3.Project(agent.desiredVelocity, transform.forward).magnitude)
    }
}