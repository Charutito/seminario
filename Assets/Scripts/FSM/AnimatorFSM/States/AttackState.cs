using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Stalking State")]
	public class AttackState : BaseState
	{
		protected override void Setup() { }

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				StateManager.Entity.Animator.SetTrigger(EntityAnimations.Attack);
				StateManager.Entity.Animator.SetInteger(EntityAnimations.RandomAttack, Random.Range(0, 2));
				StateManager.Entity.CurrentAction = GroupAction.Stalking;
			};
		}
	}
}
