using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Entity))]
    public class EntityMove : MonoBehaviour
    {
        private Entity _entity;
        private Animator _animator;

        #region Movement
        public void MoveTransform(float x, float z, bool rotate = true)
        {
            var newPosition = transform.position + (new Vector3(x, 0, z) * _entity.Stats.MoveSpeed.Actual * Time.deltaTime);

            if (rotate)
            {
                RotateTowards(newPosition);
            }

            transform.position = newPosition;
        }

        public void MoveAgent(Vector3 newPosition)
        {

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
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var axisHorizontal = Input.GetAxis("Horizontal");
            var axisVertical = Input.GetAxis("Vertical");

            if (axisVertical != 0 || axisHorizontal != 0)
            {
                MoveTransform(axisHorizontal, axisVertical);
            }

            // This too
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _entity.Stats.MoveSpeed.Actual = _entity.Stats.MoveSpeed.Max;
            }
            else
            {
                _entity.Stats.MoveSpeed.Actual = _entity.Stats.MoveSpeed.Min;
            }
        }
    }
}
