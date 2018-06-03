using Entities;
using Entities.Base;
using UnityEngine;
using Util;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Stalking State")]
	public class MineDashState : BaseState
	{
		private Vector3 _lastPosition;
		private bool _hasArrived;
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
				
				_stateManager.Entity.EntityMove.MoveAgent(_stateManager.Entity.transform.GetMaxDistancePosition(_stateManager.Entity.transform.forward));
				Debug.Log(_stateManager.Entity.transform.GetMaxDistancePosition(_stateManager.Entity.transform.forward));
			};

			OnUpdate += () =>
			{
				if (!_hasArrived && _stateManager.Entity.EntityMove.HasAgentArrived())
				{
					_hasArrived = true;
					//_stateManager.FSM.SetTrigger("Arrived");
				}
			};
			
			OnExit += () =>
			{
				_hasArrived = false;
			};
		}
	}
}
