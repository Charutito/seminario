using BattleSystem;
using UnityEngine;
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
        public EntitySounds EntitySounds { get; private set; }
        public EventFSM<int> EntityFsm { get; protected set; }
        #endregion
        
        #region Stats
        public bool IsAttacking { get; set; }
        public bool IsSpecialAttacking { get; set; }
        public bool IsDead { get { return Stats.CurrentHealth <= 0; } }
        public bool IsGod { get { return godMode; } }
        public bool IsInvulnerable { get; set; }
        public Damage LastDamage { get; private set; }
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
        [HideInInspector] public EntityDefinition Stats;
        [SerializeField] public EntityDefinition Definition;
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
            if (IsDead || (IsInvulnerable && !damage.Absolute)) return;
            
            LastDamage = damage;

            if (!IsGod || damage.Absolute)
            {
                Stats.CurrentHealth -= damage.amount;
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
        
        public virtual void Heal(int amount)
        {
            Stats.CurrentHealth += amount;
        }
        public virtual void HealEnergy(int amount)
        {
            Stats.CurrentSpirit += amount;
        }

        protected abstract void SetFsm();

        protected virtual void Awake()
        {
            TryGetComponents();
            
            if (Definition != null)
            {
                Stats = Definition.CreateInstance();
                Agent.speed = Stats.MovementSpeed;
            }
            else
            {
            #if DEBUG
                Debug.LogWarning("Entity " + gameObject.name + " has no definition attached to it.");
            #endif
            }

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
            EntitySounds = GetComponent<EntitySounds>();
        }
    }
}