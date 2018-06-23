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
				StateManager.Entity.EntityMove.MoveAgent(StateManager.Entity.Target.transform.position);

				_currentTimeToExplode -= Time.deltaTime;

                _numStr = _currentTimeToExplode.ToString("0.000", CultureInfo.InvariantCulture);
                parts = _numStr.Split('.');

                _numEntero = int.Parse(parts[0]);
                _numDecimal = int.Parse(parts[1]);

                IntTimerText.text = _numEntero.ToString();
                FloatTimerText.text = _numDecimal.ToString();

                if (_currentTimeToExplode <= 0)
				{
                    StateManager.Entity.SelfDestroy();
				}
			};

			OnExit += () =>
			{
				IntTimerText.text = string.Empty;
				FloatTimerText.text = string.Empty;
			};
		}

		private void OnTriggerEnter(Collider other)
		{
			StateManager.Entity.SelfDestroy();
		}
	}
}
