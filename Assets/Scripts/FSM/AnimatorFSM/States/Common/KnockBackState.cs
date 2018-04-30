using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/KnockBack State")]
	public class KnockBackState : BaseState
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
				_entity.Animator.SetTrigger(EntityAnimations.GettingHitBack); 
				_entity.EntityAttacker.attackArea.enabled = false;
			};
		}
	}
}
