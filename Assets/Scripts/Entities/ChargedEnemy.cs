using BattleSystem;
using FSM;
using Steering;
using UnityEngine;

namespace Entities
{
    public class ChargedEnemy : BasicEnemy
    {

        protected override void SetFsm()
        {
            EntityFsm = new ChargedEnemyFSM(this);
        }
    }
}