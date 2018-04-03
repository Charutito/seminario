using FSM;
using Managers;
using UnityEngine;
using System;


namespace Entities
{



    public class BasicRangedEnemy : BasicEnemy
    {

        [Range(0f, 10f)]
        public float RangeToAim;
        
        public float fireSpeed;
        public float recoilTime;
        public float nextFire;

        protected override void SetFsm()
        {
            EntityFsm = new BasicRangedEnemyFSM(this);
            
        }

     
    }
}

