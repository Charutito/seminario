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
        public float FireRate; 
        public float recoilTime;
        public float nextFire= 0f;
        public Transform[] PosToFlee;
        public Transform NextPos;
        
        public float fireSpeed;

        protected override void SetFsm()
        {
            EntityFsm = new BasicRangedEnemyFSM(this);
            
        }

     
    }
}

