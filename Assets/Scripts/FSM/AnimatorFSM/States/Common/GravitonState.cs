using BattleSystem;
using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    public class GravitonState : BaseState
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
                _stateManager.StateLocked = true;
                _stateManager.Entity.Animator.SetTrigger(EntityAnimations.Countered);
            };
		
            OnExit += () =>
            {
                _stateManager.StateLocked = false;
                _stateManager.Entity.CurrentAction = GroupAction.Stalking;
            };
        }
    }
}
