using FSM;
using Managers;
using UnityEngine;
using System;

namespace Entities
{
    public class BasicEnemy : GroupEntity
    {
        [Range(1f, 5f)]
        public float AttackRange = 2f;

        [Header("Stun")]
        public int hitsToGetStunned = 3;
        public float stunDuration = 0.5f;

		public event Action OnAttackRecovering = delegate { };
		public event Action OnAttackRecovered = delegate { };

        #region Local Vars
		[SerializeField] public GameObject Hitpart;
		[SerializeField] public Transform hitpos;
        private BasicEnemyFSM fsm;
        #endregion

        public void HitFeedback()
        {
            var part = Instantiate(Hitpart, hitpos.position, hitpos.rotation, hitpos);
            Destroy(part, 1);
        }

		public void AttackRecovered()
		{
			if (OnAttackRecovered != null)
			{
				OnAttackRecovered();
			}
		}

		public void AttackRecovering()
		{
			if (OnAttackRecovering != null)
			{
				OnAttackRecovering();
			}
		}

        private void Update()
        {
            fsm.Update();
        }

        private void Start()
        {
            Target = GameManager.Instance.Character;
            fsm = new BasicEnemyFSM(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}
