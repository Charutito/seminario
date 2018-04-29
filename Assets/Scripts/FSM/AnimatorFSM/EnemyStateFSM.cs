using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFSM : StateMachineBehaviour
{
	private static Dictionary<EnemyState, Type> availableStates = new Dictionary<EnemyState, Type>();
	
	public EnemyState changeTo;
	
	private MonoBehaviour currentState;
    
    static EnemyStateFSM()
	{
		availableStates.Add(EnemyState.Idle, typeof(IdleState));
		availableStates.Add(EnemyState.Patrol, typeof(PatrolState));
        availableStates.Add(EnemyState.Chase, typeof(ChaseState));
		availableStates.Add(EnemyState.Target, typeof(TargetLostState));
		
        availableStates.Add(EnemyState.Attack, typeof(AttackState));
        availableStates.Add(EnemyState.RangedAttack, typeof(RangedAttackState));
		
        availableStates.Add(EnemyState.Death, typeof(DeathState));
        availableStates.Add(EnemyState.GetHit, typeof(GetHitState));
        availableStates.Add(EnemyState.Stun, typeof(StunnedState));
    }

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		currentState = currentState ?? (animator.GetComponentInChildren(availableStates[changeTo], true) as MonoBehaviour);
		
		if(currentState != null) currentState.enabled = true;
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(currentState != null) currentState.enabled = false;
	}
}

public enum EnemyState
{
	Idle = 0,
	Patrol = 1,
	Chase = 2,
	Target = 3,
	
    Attack = 10,
    RangedAttack = 12,
    
	Death = 20,
	GetHit = 21,
	Stun = 22,
}