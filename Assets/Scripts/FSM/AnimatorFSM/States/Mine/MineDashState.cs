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
			};

			OnUpdate += () =>
			{
				_stateManager.Entity.EntityMove.MoveAgent(_stateManager.Entity.Target.transform.position);
			};
			
			OnExit += () =>
			{
				_hasArrived = false;
			};
		}
	}
}
