﻿using Entities;
using UnityEngine;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Stalking State")]
    public class StalkingState : BaseState
    {
        private BasicEnemy _entity;

        protected override void Setup()
        {
            _entity = GetComponentInParent<BasicEnemy>();
        }

        protected override void DefineState()
        {
            OnUpdate += () => _entity.EntityMove.RotateInstant(_entity.Target.transform.position);
        }
    }
}