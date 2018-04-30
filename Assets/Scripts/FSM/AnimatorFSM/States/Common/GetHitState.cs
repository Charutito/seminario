using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Get Hit State")]
    public class GetHitState : BaseState
    {
        private AbstractStateManager _stateManager;

        protected override void Setup()
        {
            _stateManager = GetComponentInParent<AbstractStateManager>();
        }

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                _stateManager.Entity.Animator.SetTrigger(EntityAnimations.GetHit);
                
                if (_stateManager.Entity.EntityAttacker != null)
                {
                    _stateManager.Entity.EntityAttacker.attackArea.enabled = false;
                }
            };
        }
    }
}
