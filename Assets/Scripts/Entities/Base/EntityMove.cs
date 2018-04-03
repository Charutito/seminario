using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class EntityMove : MonoBehaviour
    {
        private Entity _entity;

        #region Movement
        public void MoveTransform(float x, float z, bool rotate = true)
        {
            var newPosition = transform.position + (new Vector3(x, 0, z).normalized * _entity.Stats.MoveSpeed.Current * Time.deltaTime);

            if (rotate)
            {
                RotateTowards(newPosition);
            }

            transform.position = newPosition;
        }
        #endregion

        #region NavMesh Movement
        public void MoveAgent(Vector3 dest)
        {
            _entity.Agent.SetDestination(dest);
        }

        public bool HasAgentArrived()
        {
            if (!_entity.Agent.pathPending && (_entity.Agent.remainingDistance <= _entity.Agent.stoppingDistance))
            {
                return (!_entity.Agent.hasPath || _entity.Agent.velocity.sqrMagnitude <= 0f);
            }

            return false;
        }
        #endregion


        #region Rotation
        public void RotateTowards(Vector3 target, float rotationSpeed = 10f)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        public void RotateInstant(Vector3 target)
        {
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
        }
        #endregion

        private void Start()
        {
            _entity = GetComponent<Entity>();
        }

        // Animation Crap
        public void FootR() { }
        public void FootL() { }
    }
}
