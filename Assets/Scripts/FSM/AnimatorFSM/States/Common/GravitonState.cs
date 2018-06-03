using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    public class GravitonState : BaseState
    {
        protected override void Setup() { }

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                StateManager.Entity.Agent.ResetPath();
                StateManager.StateLocked = true;
                StateManager.Entity.Animator.SetTrigger(EntityAnimations.Countered);
            };
		
            OnExit += () =>
            {
                StateManager.StateLocked = false;
                StateManager.Entity.CurrentAction = GroupAction.Stalking;
            };
        }
    }
}
