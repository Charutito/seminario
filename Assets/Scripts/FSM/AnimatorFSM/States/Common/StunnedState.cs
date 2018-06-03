using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Stunned State")]
    public class StunnedState : BaseState
    {
        protected override void Setup() { }

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                StateManager.Entity.Animator.SetTrigger(EntityAnimations.Countered);
            };
        }
    }
}
