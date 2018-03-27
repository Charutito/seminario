using FSM;
using Managers;
using UnityEngine;

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
        private BasicEnemyFSM fsm;
        #endregion

        public GameObject Hitpart;
        public Transform hitpos;

        public void HitFeedback()
        {
            var part = Instantiate(Hitpart, hitpos.position, hitpos.rotation, hitpos);
            Destroy(part, 1);
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
