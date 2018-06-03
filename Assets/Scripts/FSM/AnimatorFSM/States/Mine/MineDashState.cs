using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;
using Util;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Mine Dash State")]
	public class MineDashState : BaseState
	{
		public LayerMask HitLayers;
		public float ExplosionRange = 4f;
		
		private Vector3 _lastPosition;
		private AbstractStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<AbstractStateManager>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_stateManager.Entity.EntityMove.RotateInstant(_stateManager.Entity.Target.transform.position);
			};

			OnUpdate += () =>
			{
				var characterPosition = _stateManager.Entity.Target.transform.position;
					
				_stateManager.Entity.EntityMove.MoveAgent(characterPosition);

				if (Vector3.Distance(_stateManager.Entity.transform.position, characterPosition) <= _stateManager.Entity.AttackRange)
				{
					var colliders = Physics.OverlapSphere(_stateManager.Entity.transform.position, ExplosionRange, HitLayers);

					foreach (var other in colliders)
					{
						var damageable = other.GetComponent<IDamageable>();

						if (damageable != null)
						{
							var damage = new Damage
							{
								Amount = _stateManager.Entity.Stats.LightAttackDamage,
								Displacement = 3f,
								Origin = _stateManager.Entity.transform,
								Type = DamageType.ThirdAttack
							};
							
							damageable.TakeDamage(damage);
						}
					}
					_stateManager.Entity.SelfDestroy();
				}
			};
		}
	}
}
