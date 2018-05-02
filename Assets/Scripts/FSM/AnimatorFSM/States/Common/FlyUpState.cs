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
		};
		
		OnExit += () =>
		{
			_stateManager.StateLocked = false;
			_stateManager.Entity.Agent.enabled = true;
			_stateManager.Entity.CurrentAction = GroupAction.Stalking;
		};
	}
}
