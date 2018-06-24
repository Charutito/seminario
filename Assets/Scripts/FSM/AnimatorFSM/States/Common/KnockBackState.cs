using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;
using Util;

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
					var normalizedOrigin = new Vector3(
												StateManager.LastDamage.OriginPosition.x,
												transform.position.y,
												StateManager.LastDamage.OriginPosition.z);
					
					var moveposition = Vector3.MoveTowards(transform.position, normalizedOrigin, -StateManager.LastDamage.Displacement * Random.Range(0.5f, DisplacementMultiplier));
					
					FrameUtil.AfterDelay(DisplacementTime, () => CheckIfCanGoDown());
					
					StateManager.Entity.EntityMove.SmoothMoveTransform(moveposition, DisplacementTime);
				}

				if (StateManager.Entity.EntityAttacker != null)
				{
					StateManager.Entity.EntityAttacker.attackArea.enabled = false;
				}
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
				StateManager.Entity.SelfDestroy(DamageType.Environment);
				
				var rb = StateManager.Entity.gameObject.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				
				return true;
			}

			return false;
		}
	}
}
