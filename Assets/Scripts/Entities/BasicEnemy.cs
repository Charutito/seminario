using FSM;
using Managers;
using UnityEngine;
using System.Collections;
using System;
using BattleSystem;

namespace Entities
{
    public class BasicEnemy : GroupEntity
    {
        [Range(1f, 25f)]
        public float AttackRange = 2f;
        
        [Header("Stun")]
        public int HitsToGetStunned = 3;
        
        [Range(0, 1)]
        public float DmgDispl = 0.7f;
        
        [Header("GetHit")]
        public int getHitDuration = 1;

        #region Local Vars
        [SerializeField] public GameObject Hitpart;
		[SerializeField] public Transform Hitpos;
        #endregion

        public void HitFeedback()
        {
            EntitySounds.PlayEffect("Hit", transform.position);
            
            var part = Instantiate(Hitpart, Hitpos.position, Hitpos.rotation, Hitpos);
            Destroy(part, 1);
        }
        
        public void DeathFeedback()
        {
            EntitySounds.PlayEffect("Death", transform.position);
        }
        
        public void Flash()
        {
            StartCoroutine("FlashCorroutine");
        }
        
        IEnumerator FlashCorroutine()
        {
            for (int i = 0; i < 2; i++)
            {
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Tint", Color.red);
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Tint", Color.white);
                yield return new WaitForSeconds(0.1f);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
        
        // Useless functions
        protected override void SetFsm() { }
    }
}
