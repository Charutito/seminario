using FSM;
using Managers;
using UnityEngine;
using System;
using GameUtils;
using Util;
using System.Collections;
using BattleSystem;

namespace Entities
{
    public class BasicRangedEnemy : BasicEnemy
    {
        [Range(0f, 15f)]
        public float RangeToAim;        
        public float FireRate; 
        public float recoilTime;
        public float nextFire= 0f;
        public Transform[] PosToFlee;
        public Transform NextPos;

        public GameObject bullet;
        public Transform bulletSpawnPos;

        [Range(0f, 10f)]
        public float FleeRange;

        protected override void SetFsm()
        {
            EntityFsm = new BasicRangedEnemyFSM(this);            
        }     

        public void Shot()
        {
            Instantiate(bullet, bulletSpawnPos.position, transform.rotation);    
        }
    }
}







