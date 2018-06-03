using System.Collections;
using System.Collections.Generic;
using AnimatorFSM;
using AnimatorFSM.States;
using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;

public class FlyUpState : BaseState
{
	protected override void Setup() { }

	protected override void DefineState()
	{
		OnEnter += () =>
		{
			StateManager.Entity.Agent.ResetPath();
			StateManager.Entity.Agent.enabled = false;
			StateManager.StateLocked = true;
			StateManager.Entity.Animator.SetTrigger(EntityAnimations.FlyUp);
			
			if (StateManager.LastDamage.Displacement > 0f)
			{
				StateManager.Entity.EntityMove.SmoothMoveTransform(
					Vector3.MoveTowards(transform.position, StateManager.LastDamage.Origin.position, -StateManager.LastDamage.Displacement), 0.1f);
			}
		};
		
		OnExit += () =>
		{
			StateManager.StateLocked = false;
			StateManager.Entity.Agent.enabled = true;
			StateManager.Entity.CurrentAction = GroupAction.Stalking;
		};
	}
}
