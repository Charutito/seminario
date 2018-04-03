using FSM;
using Managers;
using UnityEngine;
using System;


namespace Entities
{



    public class BasicRangedEnemy : BasicEnemy
    {

        [Range(0f, 5f)]
        public float RangeToAim;
        [Range(0.5f, 5f)]
        public float MaxAimTime;
        public float AttackSpeed;

        protected override void SetFsm()
        {
            EntityFsm = new BasicRangedEnemyFSM(this);
            
        }

     
    }
}

