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
		private AbstractStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponent<AbstractStateManager>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				if(_stateManager.Entity.Agent.enabled) _stateManager.Entity.Agent.ResetPath();
				_stateManager.Entity.Agent.enabled = false;
				_stateManager.Entity.Animator.SetTrigger(EntityAnimations.GettingHitBack);

				if (_stateManager.LastDamage.Displacement > 0f)
				{
					_stateManager.Entity.EntityMove.SmoothMoveTransform(
						Vector3.MoveTowards(transform.position, _stateManager.LastDamage.origin.position, -_stateManager.LastDamage.Displacement * Random.Range(0.5f, DisplacementMultiplier)),
						DisplacementTime, CheckIfCanGoDown);
				}
				
				if (_stateManager.Entity.EntityAttacker != null) _stateManager.Entity.EntityAttacker.attackArea.enabled = false;
			};
			
			OnExit += () =>
			{
				_stateManager.Entity.CurrentAction = GroupAction.Stalking;
				
				if(!_stateManager.Entity.IsDead)
				_stateManager.Entity.Agent.enabled = true;
			};
		}

		private void CheckIfCanGoDown()
		{
			if (!_stateManager.Entity.EntityMove.IsAgentOnNavMesh())
			{
				var rb = _stateManager.Entity.gameObject.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				
				_stateManager.Entity.TakeDamage(new Damage
				{
					amount = (int)_stateManager.Entity.Stats.Health.Max,
					type = DamageType.Environment,
					origin = _stateManager.Entity.transform,
					Absolute = true
				});
			}
		}
	}
}
