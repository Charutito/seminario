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
		public Text IntTimerText;
        public Text FloatTimerText;
        private string _numStr;
        private int _numEntero;
        private int _numDecimal;
        string[] parts;

        private float _currentTimeToExplode;
		private Vector3 _lastPosition;

		protected override void Setup()
		{
			_currentTimeToExplode = TimeToExplode;
        }

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				StateManager.Entity.EntityMove.RotateInstant(StateManager.Entity.Target.transform.position);
			};

			OnUpdate += () =>
			{
				var characterPosition = StateManager.Entity.Target.transform.position;
					
				StateManager.Entity.EntityMove.MoveAgent(characterPosition);

				_currentTimeToExplode -= Time.deltaTime;

                _numStr = _currentTimeToExplode.ToString("0.000", CultureInfo.InvariantCulture);
                parts = _numStr.Split('.');

                _numEntero = int.Parse(parts[0]);
                _numDecimal = int.Parse(parts[1]);

                IntTimerText.text = _numEntero.ToString();
                FloatTimerText.text = _numDecimal.ToString();

                //Math.Round(_numDecimal, 1).ToString(CultureInfo.InvariantCulture);

                if (_currentTimeToExplode <= 0 || Vector3.Distance(StateManager.Entity.transform.position, characterPosition) <= StateManager.Entity.AttackRange)
				{
					var colliders = Physics.OverlapSphere(StateManager.Entity.transform.position, ExplosionRange, HitLayers);

					foreach (var other in colliders)
					{
						var damageable = other.GetComponent<IDamageable>();

						if (damageable != null)
						{
							var damage = new Damage
							{
								Amount = StateManager.Entity.Stats.LightAttackDamage,
								Displacement = ExplosionDisplacement,
								OriginPosition = StateManager.Entity.transform.position,
								OriginRotation = StateManager.Entity.transform.rotation,
								Type = DamageType.ThirdAttack
							};
							
							damageable.TakeDamage(damage);
						}
					}

                    IntTimerText.text = string.Empty;
                    FloatTimerText.text = string.Empty;

                    StateManager.Entity.SelfDestroy();
				}
			};
		}

		private void OnTriggerEnter(Collider other)
		{
			_currentTimeToExplode = 0;
		}
	}
}
