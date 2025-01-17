﻿using System;
using System.Collections.Generic;
using AnimatorFSM.States;
using UnityEngine;
using Util;

namespace AnimatorFSM
{
	public class EnemyStateFSM : StateMachineBehaviour
	{
		private static readonly Dictionary<EnemyState, Type> AvailableStates = new Dictionary<EnemyState, Type>();
	
		public EnemyState changeTo;
	
		private MonoBehaviour _currentState;
    
		static EnemyStateFSM()
		{
			AvailableStates.Add(EnemyState.Idle, typeof(IdleState));
			AvailableStates.Add(EnemyState.Patrol, typeof(PatrolState));
			AvailableStates.Add(EnemyState.Chase, typeof(ChaseState));
			AvailableStates.Add(EnemyState.Target, typeof(TargetLostState));
			AvailableStates.Add(EnemyState.Stalk, typeof(StalkingState));
			AvailableStates.Add(EnemyState.Flee, typeof(FleeToPositionState));
			AvailableStates.Add(EnemyState.RangedStalk, typeof(RangedStalkingState));
		
			AvailableStates.Add(EnemyState.Attack, typeof(AttackState));
			AvailableStates.Add(EnemyState.RangedAttack, typeof(RangedAttackState));
			AvailableStates.Add(EnemyState.Aim, typeof(AimTargetState));
			AvailableStates.Add(EnemyState.BomberAttack, typeof(BomberAttackState));
			AvailableStates.Add(EnemyState.Shield, typeof(ShieldState));
			AvailableStates.Add(EnemyState.MineDash, typeof(MineDashState));
		
			AvailableStates.Add(EnemyState.Death, typeof(DeathState));
			AvailableStates.Add(EnemyState.GetHit, typeof(GetHitState));
			AvailableStates.Add(EnemyState.Stun, typeof(StunnedState));
			AvailableStates.Add(EnemyState.KnockBack, typeof(KnockBackState));
			AvailableStates.Add(EnemyState.FlyUp, typeof(FlyUpState));
			AvailableStates.Add(EnemyState.Graviton, typeof(GravitonState));
		}

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_currentState = _currentState ?? (animator.GetComponentInChildren(AvailableStates[changeTo], true) as MonoBehaviour);

			if (_currentState != null)
			{
				_currentState.enabled = false;
				
				FrameUtil.OnNextFrame(() =>
				{
					_currentState.enabled = true;
				});
				
			}
			else
			{
				Debug.LogWarning(string.Format( "The state [{0}] is not attached to the entity [{1}]",  changeTo, animator.transform.parent.name));
			}
		}

		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(_currentState != null) _currentState.enabled = false;
		}
	}

	public enum EnemyState
	{
		Idle = 0,
		Patrol = 1,
		Chase = 2,
		Target = 3,
		Stalk = 4,
		Flee = 5,
		RangedStalk = 6,
		MineDash = 7,
	
		Attack = 10,
		RangedAttack = 11,
		Aim = 12,
		BomberAttack = 13,
		Shield = 14,
    
		Death = 20,
		GetHit = 21,
		Stun = 22,
		KnockBack = 23,
		FlyUp = 24,
		Graviton = 25,
	}
}