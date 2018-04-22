using BattleSystem;
using FSM;
using Steering;
using UnityEngine;

namespace Entities
{
    public class ChargedEnemy : BasicEnemy
    {
        public float ChargeTime;
        public float recoveryTime;
        protected override void SetFsm()
        {
            EntityFsm = new ChargedEnemyFSM(this);
        
        }
    }
}