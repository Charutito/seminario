using System;
using System.Collections.Generic;
using AnimatorFSM.States;
using UnityEngine;

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
		
			AvailableStates.Add(EnemyState.Attack, typeof(AttackState));
			AvailableStates.Add(EnemyState.RangedAttack, typeof(RangedAttackState));
		
			AvailableStates.Add(EnemyState.Death, typeof(DeathState));
			AvailableStates.Add(EnemyState.GetHit, typeof(GetHitState));
			AvailableStates.Add(EnemyState.Stun, typeof(StunnedState));
			AvailableStates.Add(EnemyState.KnockBack, typeof(KnockBackState));
			AvailableStates.Add(EnemyState.FlyUp, typeof(FlyUpState));
		}

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_currentState = _currentState ?? (animator.GetComponentInChildren(AvailableStates[changeTo], true) as MonoBehaviour);
		
			if(_currentState != null) _currentState.enabled = true;
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
	
		Attack = 10,
		RangedAttack = 12,
    
		Death = 20,
		GetHit = 21,
		Stun = 22,
		KnockBack = 23,
		FlyUp = 24,
	}
}