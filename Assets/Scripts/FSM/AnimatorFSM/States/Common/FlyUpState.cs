using System.Collections;
using System.Collections.Generic;
using AnimatorFSM;
using AnimatorFSM.States;
using BattleSystem;
using Entities;
using UnityEngine;

public class FlyUpState : BaseState
{
	private BasicEnemyStateManager _stateManager;

	protected override void Setup()
	{
		_stateManager = GetComponent<BasicEnemyStateManager>();
	}

	protected override void DefineState()
	{
		OnEnter += () =>
		{
			_stateManager.Entity.Agent.enabled = false;
			_stateManager.StateLocked = true;
			_stateManager.Entity.Animator.SetTrigger("FlyUp");
		};
		
		OnExit += () =>
		{
			_stateManager.StateLocked = false;
			_stateManager.Entity.Agent.enabled = true;
			_stateManager.Entity.CurrentAction = GroupAction.Stalking;
		};
	}
}
