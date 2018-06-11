using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.MovingFloor
{
    public class Floor : MonoBehaviour
    {
        public FloorConfig Config;
        
        [HideInInspector]
        public List<Transform> Targets;
        
        public event Action OnFloorActivate = delegate { };
        public event Action OnFloorDeactivate = delegate { };
        public event Action OnLateUpdate = delegate { };

        private bool _isActive = false;

        private void LateUpdate()
        {
            OnLateUpdate.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Targets.Contains(other.transform))
            {
                Targets.Add(other.transform);

                if (!_isActive)
                {
                    _isActive = true;
                    OnFloorActivate.Invoke();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Targets.Contains(other.transform))
            {
                Targets.Remove(other.transform);

                if (Targets.Count <= 0)
                {
                    _isActive = false;
                    OnFloorDeactivate.Invoke();
                }
            }
        }
    }
}
