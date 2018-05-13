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
	private AbstractStateManager _stateManager;

	protected override void Setup()
	{
		_stateManager = GetComponent<AbstractStateManager>();
	}

	protected override void DefineState()
	{
		OnEnter += () =>
		{
			_stateManager.Entity.Agent.ResetPath();
			_stateManager.Entity.Agent.enabled = false;
			_stateManager.StateLocked = true;
			_stateManager.Entity.Animator.SetTrigger(EntityAnimations.FlyUp);
			
			if (_stateManager.LastDamage.Displacement > 0f)
			{
				_stateManager.Entity.EntityMove.SmoothMoveTransform(
					Vector3.MoveTowards(transform.position, _stateManager.LastDamage.origin.position, -_stateManager.LastDamage.Displacement), 0.1f);
			}
		};
		
		OnExit += () =>
		{
			_stateManager.StateLocked = false;
			_stateManager.Entity.Agent.enabled = true;
			_stateManager.Entity.CurrentAction = GroupAction.Stalking;
		};
	}
}
