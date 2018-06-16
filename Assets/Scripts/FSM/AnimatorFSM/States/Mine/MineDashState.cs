using System;
using System.Globalization;
using System.Timers;
using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Mine Dash State")]
	public class MineDashState : BaseState
	{
		public LayerMask HitLayers;
		public float ExplosionRange = 4f;
		public float ExplosionDisplacement = 2f;

		[Header("Timer")]
		public float TimeToExplode = 10f;
		public Text TimerText;

		private float _currentTimeToExplode;
		private Vector3 _lastPosition;
		private AbstractStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<AbstractStateManager>();
			_currentTimeToExplode = TimeToExplode;
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

				_currentTimeToExplode -= Time.deltaTime;
				TimerText.text = Math.Round(_currentTimeToExplode, 1).ToString(CultureInfo.InvariantCulture);

				if (_currentTimeToExplode <= 0 || Vector3.Distance(_stateManager.Entity.transform.position, characterPosition) <= _stateManager.Entity.AttackRange)
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
								Displacement = ExplosionDisplacement,
								Origin = _stateManager.Entity.transform,
								Type = DamageType.ThirdAttack
							};
							
							damageable.TakeDamage(damage);
						}
					}

					TimerText.text = string.Empty;
					_stateManager.Entity.SelfDestroy();
				}
			};
		}

		private void OnTriggerEnter(Collider other)
		{
			_currentTimeToExplode = 0;
		}
	}
}
