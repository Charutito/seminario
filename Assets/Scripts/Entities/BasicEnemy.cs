using FSM;
using Managers;
using UnityEngine;
using System;
using BattleSystem;

namespace Entities
{
    public class BasicEnemy : GroupEntity
    {
        [Range(1f, 15f)]
        public float AttackRange = 2f;

        [Header("Stun")]
        public int hitsToGetStunned = 3;
        public float stunDuration = 0.5f;
        [Range(0, 1)]
        public float DmgDispl = 0.7f;
        [Header("GetHit")]
        public int getHitDuration = 1;
        [Header("GettingHitBack")]
        public int getHitBackDuration = 2;
        
        #region Local Vars
        [SerializeField] public GameObject Hitpart;
		[SerializeField] public Transform hitpos;
        #endregion

        public void DmgDisp()
        {
            this.EntityMove.SmoothMoveTransform(transform.position -transform.forward * DmgDispl, 0.1f);
        }

        public void HitFeedback()
        {
            var part = Instantiate(Hitpart, hitpos.position, hitpos.rotation, hitpos);
            DmgDisp();
            Destroy(part, 1);
        }

        protected override void SetFsm()
	    {
		    EntityFsm = new BasicEnemyFSM(this);
	        OnDeath += entity => Destroy(gameObject, 2);
	    }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}
