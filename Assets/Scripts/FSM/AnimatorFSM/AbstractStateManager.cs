using System;
using BattleSystem;
using Entities;
using UnityEngine;

namespace AnimatorFSM
{
	public class AbstractStateManager : MonoBehaviour
	{
		public bool StateLocked { get; set; }
		[HideInInspector]
		public Damage LastDamage;
		
		public BasicEnemy Entity { get; private set; }
		public Animator FSM { get; private set; }
		
		protected int CurrentHitsToStun;

		protected virtual void OnEntityDamage(Damage damage)
		{
			Entity.HitFeedback();
			LastDamage = damage;

			if (CurrentHitsToStun >= Entity.HitsToGetStunned)
			{
				damage.Type = DamageType.Block;
			}
			
			switch (damage.Type)
			{
				case DamageType.Block:
					SetState("Stun");
					CurrentHitsToStun = 0;
					break;
				case DamageType.ThirdAttack:
					SetState("KnockBack", true);
					break;
				case DamageType.FlyUp:
					SetState("FlyUp", true);
					break;
				case DamageType.Graviton:
					SetState("Graviton", true);
					break;
				default:
					CurrentHitsToStun++;
					SetState("GetHit");
					break;
			}
		}

		protected virtual void OnEntityDeath(Entity entity)
		{
			Entity.HitFeedback();
			Entity.DeathFeedback();
			Entity.OnTakeDamage -= OnEntityDamage;
			Entity.OnDeath -= OnEntityDeath;
			SetState("Death", true);
		}

		protected void SetState(string state, bool force = false)
		{
			if(!StateLocked || force) FSM.SetTrigger(state);
		}

		private void Awake()
		{
			Entity = GetComponentInParent<BasicEnemy>();
			FSM = GetComponent<Animator>();

			Entity.OnTakeDamage += OnEntityDamage;
			Entity.OnDeath += OnEntityDeath;
		}
	}
}
