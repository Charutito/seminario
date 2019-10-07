using GameUtils;
using UnityEngine;

namespace Managers.Camera
{
    public class CameraFollow : SingletonObject<CameraFollow>
    {
        [SerializeField]
        private float moveSpeed = 3f;
        
        private Transform _target;

        private void Start()
        {
            _target = GameManager.Instance.Character.transform;
        }

        private void SetTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * moveSpeed); 
        }
    }
}

