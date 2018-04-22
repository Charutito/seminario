using UnityEngine;

namespace Utility
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float TimeToDestroy = 1f;

        public GameObject DeathEffect;

        private void Awake()
        {
            Destroy(gameObject, TimeToDestroy);
        }

        private void OnDestroy()
        {
            if (DeathEffect != null) Instantiate(DeathEffect, transform.position, transform.rotation);
        }
    }
}

