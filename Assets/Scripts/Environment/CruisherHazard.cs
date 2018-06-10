using BattleSystem;
using Entities;
using UnityEngine;

namespace Environment
{
    public class CruisherHazard : MonoBehaviour
    {
        public Transform RightWall;
        public Transform LeftWall;
    
        [Range(0, 3)]
        public float DistanceToKill;

        private void OnTriggerStay(Collider other)
        {
            var distance = Vector3.Distance(RightWall.transform.position, LeftWall.transform.position);

            if (distance <= DistanceToKill)
            {
                var entity = other.GetComponent<Entity>();

                if (entity != null && !entity.IsDead)
                {
                    entity.Agent.ResetPath();
                    entity.Agent.enabled = false;
                    
                    entity.TakeDamage(new Damage
                    {
                        Amount = entity.Definition.MaxHealth,
                        Type = DamageType.Unknown,
                        Origin = transform,
                        Absolute = true
                    });
                }
            }
        }
    }
}
