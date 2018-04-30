using Entities;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Get Hit State")]
    public class GetHitState : BaseState
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
                _entity.Animator.SetTrigger(EntityAnimations.GetHit); 
                _entity.EntityAttacker.attackArea.enabled = false;
            };
        }
    }
}
