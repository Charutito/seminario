using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    public class GetHitState : BaseState
    {
        public float DisplacementMultiplier = 1f;
        public float DisplacementTime = 0.1f;
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
                _stateManager.Entity.Animator.SetTrigger(EntityAnimations.GetHit);
                
                _stateManager.Entity.EntityMove.SmoothMoveTransform(
                    Vector3.MoveTowards(transform.position, _stateManager.LastDamage.origin.position, -_stateManager.LastDamage.Displacement * DisplacementMultiplier), DisplacementTime);
                
                if (_stateManager.Entity.EntityAttacker != null) _stateManager.Entity.EntityAttacker.attackArea.enabled = false;
            };
        }
    }
}
