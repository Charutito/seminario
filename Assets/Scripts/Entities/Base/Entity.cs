using BattleSystem;
using UnityEngine;
using Stats;
using System;
using UnityEngine.AI;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        #region Properties
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; }
        public Collider Collider { get; private set; }
        public bool IsDead { get { return stats.Health.Current <= stats.Health.Min; } }
        public bool IsGod { get { return godMode; } }
        public EntityStats Stats { get { return stats; } }
        #endregion


        #region Events
        public event Action OnAnimUnlock = delegate { };
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
            if (!godMode)
            {
                Stats.Health.Current -= damage;
            }

            if (IsDead && OnDeath != null)
            {
                OnDeath(this);
            }
            else if (!IsDead && OnTakeDamage != null)
            {
                OnTakeDamage(damage, type);
            }
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

        protected virtual void Update() {}

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Collider = GetComponent<Collider>();

            Stats.Health.Current = stats.Health.Max;
        }
    }
}