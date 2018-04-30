using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Stunned State")]
    public class StunnedState : BaseState
    {
        private BasicEnemy _entity;

        protected override void Setup()
        {
            _entity = GetComponentInParent<BasicEnemy>();
        }

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                _entity.Animator.SetTrigger(EntityAnimations.Countered);
            };
        }
    }
}
