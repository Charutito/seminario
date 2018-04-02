using FSM;
using Managers;
using UnityEngine;
using System;
using BattleSystem;

namespace Entities
{
    public class BasicEnemy : GroupEntity
    {
        [Range(1f, 5f)]
        public float AttackRange = 2f;

        [Header("Stun")]
        public int hitsToGetStunned = 3;
        public float stunDuration = 0.5f;

        #region Local Vars
		[SerializeField] public GameObject Hitpart;
		[SerializeField] public Transform hitpos;
        #endregion

        public void HitFeedback()
        {
            var part = Instantiate(Hitpart, hitpos.position, hitpos.rotation, hitpos);
            Destroy(part, 1);
        }

	    public override void TakeDamage(int damage, DamageType type)
	    {
		    if (type == DamageType.SpecialAttack ||
		        (IsAttacking && (type == DamageType.Attack)))
		    {
			    type = DamageType.Block;
			    damage = 0;
		    }

		    base.TakeDamage(damage, type);
	    }

        private void Start()
        {
            Target = GameManager.Instance.Character;
            EntityFsm = new BasicEnemyFSM(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}
