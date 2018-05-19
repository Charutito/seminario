using System;
using BattleSystem;
using Entities;
using UnityEngine;

namespace AnimatorFSM
{
    public class BasicRangedStateManager : AbstractStateManager
    {
        private void Update()
        {
            FSM.SetBool("TargetInAttackRange", !Entity.Target.IsDead && Vector3.Distance(Entity.transform.position, Entity.Target.transform.position) <= Entity.AttackRange);
        }
    }
}
