using System.Collections.Generic;
using System.Linq;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	public class FleeToPositionState : BaseState
	{
		public float FleeRange = 10f;
		public float DistanceToFee = 8f;
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
				var characterDistance = Vector3.Distance(_stateManager.Entity.transform.position,
					_stateManager.Entity.Target.transform.position);
				
				if (DistanceToFee > characterDistance || _stateManager.Entity.AttackRange < characterDistance)
				{
					var newPoint = FleePoints
						.Where(x => Vector3.Distance(x.position, _stateManager.Entity.transform.position) < FleeRange) // Max distance to flee
						.Where(x => Vector3.Distance(x.position, _stateManager.Entity.Target.transform.position) < _stateManager.Entity.AttackRange) // Where the character its inside attack range too
						.OrderByDescending(x => Vector3.Distance(x.position, _stateManager.Entity.Target.transform.position))
						.FirstOrDefault();

					if (newPoint != _lastPoint)
					{
						_lastPoint = newPoint;
						_stateManager.Entity.Animator.SetFloat(EntityAnimations.Move, 1);
						if (newPoint != null) _stateManager.Entity.EntityMove.MoveAgent(newPoint.position);
					}
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
			
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, FleeRange);
			
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position, DistanceToFee);
		}
	}
}
