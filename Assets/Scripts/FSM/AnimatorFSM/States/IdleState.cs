using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Idle State")]
    public class IdleState : BaseState
    {
        private AbstractStateManager _stateManager;

        protected override void Setup()
        {
            _stateManager = GetComponentInParent<AbstractStateManager>();
        }

        protected override void DefineState()
        {
            OnEnter += () => _stateManager.Entity.Animator.SetBool(EntityAnimations.Relax, true);
            OnExit += () => _stateManager.Entity.Animator.SetBool(EntityAnimations.Relax, false);
        }
    }
}
