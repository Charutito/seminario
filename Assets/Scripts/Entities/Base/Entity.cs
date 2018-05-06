using BattleSystem;
using UnityEngine;
using Stats;
using System;
using FSM;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Entities
{
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        #region EntityComponents
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator { get; private set; }
        public Collider Collider { get; private set; }
        public EntityAttacker EntityAttacker { get; private set; }
        public EntityMove EntityMove { get; private set; }
        public EventFSM<int> EntityFsm { get; protected set; }
        #endregion
        
        #region Stats
        public bool IsAttacking { get; set; }
        public bool IsSpecialAttacking { get; set; }
        public bool IsBlocking { get; set; }
        public bool IsDead { get { return stats.Health.Current <= stats.Health.Min; } }
        public bool IsGod { get { return godMode; } }
        public EntityStats Stats { get { return stats; } }
        public int AttackDamage { get { return attackDamage; } }
        public int HeavyAttackDamage { get { return heavyAttackDamage; } }
        #endregion


        #region Events
        public event Action OnAttackRecovering = delegate { };
        public event Action OnAttackRecovered = delegate { };
        public event Action<Damage> OnTakeDamage = delegate { };
        public event Action<Entity> OnDeath = delegate { };
        public UnityEvent OnEntityDie;
        public UnityEvent OnEntityDamage;
        #endregion


        #region Local Vars
        [SerializeField] private EntityStats stats;
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private int heavyAttackDamage = 25;
        [SerializeField] private bool godMode = false;
        #endregion 
        
        
        #region Animation Events
        public virtual void AttackRecovered()
        {
            if (OnAttackRecovered != null)
            {
                OnAttackRecovered();
            }
        }

        public virtual void AttackRecovering()
        {
            if (OnAttackRecovering != null)
            {
                OnAttackRecovering();
            }
        }
        #endregion


        #region IDamageable
        public virtual void TakeDamage(Damage damage)
        {
            Debug.Log(JsonUtility.ToJson(damage));
            
            if (IsDead) return;

            if (!IsGod)
            {
                Stats.Health.Current -= damage.amount;
            }

            if (IsDead && OnDeath != null)
            {
                OnDeath(this);
                OnEntityDie.Invoke();
            }

            if (OnTakeDamage != null)
            {
                OnTakeDamage(damage);
                OnEntityDamage.Invoke();
            }
        }
        #endregion

        protected abstract void SetFsm();

        protected virtual void Awake()
        {
            Stats.Health.Current = stats.Health.Max;
            
            TryGetComponents();

            SetFsm();
        }

        protected virtual void Update()
        {
            if (EntityFsm != null)
            {
                EntityFsm.Update();
            }
        }

        private void TryGetComponents()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Collider = GetComponent<Collider>();
            EntityAttacker = GetComponent<EntityAttacker>();
            EntityMove = GetComponent<EntityMove>();
        }
    }
}