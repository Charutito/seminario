using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Stalking State")]
	public class ChaseState : BaseState
	{
		protected override void Setup() { }

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				StateManager.Entity.Animator.SetFloat(EntityAnimations.Move, 1);
				StateManager.Entity.Animator.SetInteger(EntityAnimations.RandomRun, Random.Range(0, 2));
			};

			OnUpdate += () =>
			{
				StateManager.Entity.EntityMove.RotateInstant(StateManager.Entity.Target.transform.position);
				StateManager.Entity.EntityMove.MoveAgent(StateManager.Entity.Target.transform.position);
			};
			
			OnExit += () =>
			{
				if (StateManager.Entity != null)
				{
					StateManager.Entity.Animator.SetFloat(EntityAnimations.Move, 0);
				}
			};
		}
	}
}
