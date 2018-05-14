using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Stalking State")]
	public class ChaseState : BaseState
	{
		private BasicEnemy _entity;

		protected override void Setup()
		{
			_entity = GetComponentInParent<BasicEnemy>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_entity.Animator.SetFloat(EntityAnimations.Move, 1);
			};

			OnUpdate += () =>
			{
				_entity.EntityMove.RotateInstant(_entity.Target.transform.position);
				_entity.EntityMove.MoveAgent(_entity.Target.transform.position);
			};
			
			OnExit += () =>
			{
				if(_entity != null)
				_entity.Animator.SetFloat(EntityAnimations.Move, 0);
			};
		}
	}
}
