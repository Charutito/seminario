using BattleSystem;
using UnityEngine;

namespace AnimatorFSM
{
    public class BasicEnemyStateManager : AbstractStateManager
    {
        private int _currentHitsToStun;

        private void Start()
        {
            Entity.OnSetAction += (newAction, lastAction) =>
            {
                switch (newAction)
                {
                    case GroupAction.Attacking:
                    case GroupAction.SpecialAttack:
                        SetState("Attack");
                        break;
                    case GroupAction.Stalking:
                        FSM.SetBool("Activated", true);
                        break;
                }
            };
        }

        private void Update()
        {
            FSM.SetBool("TargetInAttackRange", Vector3.Distance(Entity.transform.position, Entity.Target.transform.position) <= Entity.AttackRange);
        }
    }
}
