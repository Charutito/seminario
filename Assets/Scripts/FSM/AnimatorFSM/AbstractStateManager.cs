using System;
using BattleSystem;
using Entities;
using UnityEngine;

namespace AnimatorFSM
{
	public class AbstractStateManager : MonoBehaviour
	{
		public bool StateLocked { get; set; }
		
		public BasicEnemy Entity { get; private set; }
		public Animator FSM { get; private set; }
		
		protected int CurrentHitsToStun;

		protected virtual void OnEntityDamage(int amount, DamageType type)
		{ 
			
			Entity.HitFeedback();

			if (CurrentHitsToStun >= Entity.HitsToGetStunned)
			{
				type = DamageType.Block;
			}
			
			switch (type)
			{
				case DamageType.Block:
					SetState("Stun");
					CurrentHitsToStun = 0;
					break;
				case DamageType.ThirdAttack:
					SetState("KnockBack", true);
					break;
				case DamageType.FlyUp:
					SetState("FlyUp");
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
