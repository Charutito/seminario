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
        
        [Header("GetHit")]
        public GameObject Hitpart;
		public Transform Hitpos;

        public void HitFeedback()
        {
            EntitySounds.PlayEffect("Hit", transform.position);

            if (Hitpart != null && Hitpos != null)
            {
                var part = Instantiate(Hitpart, Hitpos.position, Hitpos.rotation, Hitpos);
                Destroy(part, 1);
            }
        }
        
        public void DeathFeedback()
        {
            if (LastDamage != null && LastDamage.Type == DamageType.Environment)
            {
                EntitySounds.PlayEffect("Fall", transform.position);
            }
            else
            {
                EntitySounds.PlayEffect("Death", transform.position);
            }
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
