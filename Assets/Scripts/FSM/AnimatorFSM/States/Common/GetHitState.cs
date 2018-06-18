using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    public class GetHitState : BaseState
    {
        public float DisplacementMultiplier = 1f;
        public float DisplacementTime = 0.1f;

        protected override void Setup() { }

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                if(StateManager.Entity.Agent.enabled) StateManager.Entity.Agent.ResetPath();
                StateManager.Entity.Animator.SetTrigger(EntityAnimations.GetHit);
                
                StateManager.Entity.EntityMove.SmoothMoveTransform(
                    Vector3.MoveTowards(transform.position, StateManager.LastDamage.OriginPosition, -StateManager.LastDamage.Displacement * DisplacementMultiplier), DisplacementTime);
                
                if (StateManager.Entity.EntityAttacker != null) StateManager.Entity.EntityAttacker.attackArea.enabled = false;
            };
        }
    }
}
