using System.Collections.Generic;
using System.Linq;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	public class FleeToPositionState : BaseState
	{
		public List<Transform> FleePoints;
		
		private Transform _lastPoint;
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
				var newPoint = FleePoints
								.OrderByDescending(x => Vector3.Distance(x.position, _stateManager.Entity.Target.transform.position))
								.FirstOrDefault();

				if (newPoint != _lastPoint)
				{
					_lastPoint = newPoint;
					_stateManager.Entity.Animator.SetFloat(EntityAnimations.Move, 1);
					_stateManager.Entity.EntityMove.MoveAgent(newPoint.position);
				}
			};

			OnUpdate += () =>
			{
				if (!_hasArrived && _stateManager.Entity.EntityMove.HasAgentArrived())
				{
					_hasArrived = true;
					_stateManager.FSM.SetTrigger("Arrived");
				}
			};

			OnExit += () =>
			{
				_hasArrived = false;
				_stateManager.Entity.Animator.SetFloat(EntityAnimations.Move, 0);
			};
		}

		private void OnDrawGizmosSelected()
		{
			if (_lastPoint != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(_lastPoint.position, 1);
			}
		}
	}
}
