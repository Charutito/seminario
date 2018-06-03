using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	public class KnockBackState : BaseState
	{
		public float DisplacementMultiplier = 1f;
		public float DisplacementTime = 0.3f;

		protected override void Setup() { }

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				if(StateManager.Entity.Agent.enabled) StateManager.Entity.Agent.ResetPath();
				StateManager.Entity.Agent.enabled = false;
				StateManager.Entity.Animator.SetTrigger(EntityAnimations.GettingHitBack);

				if (StateManager.LastDamage.Displacement > 0f)
				{
					StateManager.Entity.EntityMove.SmoothMoveTransform(
						Vector3.MoveTowards(transform.position, StateManager.LastDamage.Origin.position, -StateManager.LastDamage.Displacement * Random.Range(0.5f, DisplacementMultiplier)),
						DisplacementTime, () => CheckIfCanGoDown());
				}
				
				if (StateManager.Entity.EntityAttacker != null) StateManager.Entity.EntityAttacker.attackArea.enabled = false;
			};
			
			OnExit += () =>
			{
				StateManager.Entity.CurrentAction = GroupAction.Stalking;

				if (!StateManager.Entity.IsDead && !CheckIfCanGoDown())
				{
					StateManager.Entity.Agent.enabled = true;
				}
			};
		}

		private bool CheckIfCanGoDown()
		{
			if (!StateManager.Entity.EntityMove.CanReachPosition(StateManager.Entity.transform.position))
			{
				var rb = StateManager.Entity.gameObject.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				
				StateManager.Entity.TakeDamage(new Damage
				{
					Amount = StateManager.Entity.Stats.MaxHealth,
					Type = DamageType.Environment,
					Origin = StateManager.Entity.transform,
					Absolute = true
				});
				return true;
			}

			return false;
		}
	}
}
