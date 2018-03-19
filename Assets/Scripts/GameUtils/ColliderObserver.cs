using System;
using UnityEngine;

namespace GameUtils
{
    public class ColliderObserver : MonoBehaviour
    {
        #region Events
        public event Action<Collision> CollisionEnter = delegate { };
        public event Action<Collision> CollisionStay = delegate { };
        public event Action<Collision> CollisionExit = delegate { };

        public event Action<Collider> TriggerEnter = delegate { };
        public event Action<Collider> TriggerStay = delegate { };
        public event Action<Collider> TriggerExit = delegate { };
        #endregion

        #region Collision
        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnter(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            CollisionStay(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionExit(collision);
        }
        #endregion

        #region Trigger
        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter(other);
        }

        private void OnTriggernStay(Collider other)
        {
            TriggerStay(other);
        }
        private void OnTriggerExit(Collider other)
        {
            TriggerExit(other);
        }
        #endregion
    }
}
