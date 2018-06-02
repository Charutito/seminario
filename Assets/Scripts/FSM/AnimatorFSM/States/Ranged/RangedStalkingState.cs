using BattleSystem;
using Entities;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Ranged Stalking State")]
    public class RangedStalkingState : BaseState
    {
        private BasicEnemy _entity;

        protected override void Setup()
        {
            _entity = GetComponentInParent<BasicEnemy>();
        }

        protected override void DefineState()
        {
            OnEnter += () => _entity.CurrentAction = GroupAction.Stalking;
            OnUpdate += () => _entity.EntityMove.RotateInstant(_entity.Target.transform.position);
        }
    }
}
