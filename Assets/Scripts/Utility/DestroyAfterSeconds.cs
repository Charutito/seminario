using UnityEngine;

namespace Utility
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float TimeToDestroy = 1f;

        public GameObject DeathEffect;
        public AudioEvent DeathSound;

        private void Awake()
        {
            Destroy(gameObject, TimeToDestroy);
        }

        private void OnDestroy()
        {
            if (DeathEffect != null)
            {
                var effect = Instantiate(DeathEffect, transform.position, transform.rotation);
                Destroy(effect.gameObject, 1);
            }

            if (DeathSound != null)
            {
                DeathSound.PlayAtPoint(transform.position);
            }
        }
    }
}

