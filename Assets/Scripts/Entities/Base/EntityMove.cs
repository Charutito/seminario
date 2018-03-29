using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    [RequireComponent(typeof(Entity))]
    public class EntityMove : MonoBehaviour
    {
        private Entity _entity;
        private NavMeshAgent _agent;

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
            _agent.SetDestination(dest);
        }

        public bool HasAgentArrived()
        {
            if (!_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance))
            {
                return (!_agent.hasPath || _agent.velocity.sqrMagnitude <= 0f);
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
            _agent = GetComponent<NavMeshAgent>();
            _entity = GetComponent<Entity>();
        }

        // Animation Crap
        public void FootR() { }
        public void FootL() { }
    }
}
