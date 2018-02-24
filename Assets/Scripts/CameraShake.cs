using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Camera
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float shakeDecay = 0.2f;

        private bool isShaking = false;

        public void Shake(float intensity)
        {
            if (!isShaking)
            {
                isShaking = true;
                StartCoroutine(DoShake(transform.localPosition, transform.localRotation, intensity));
            }
        }

        private IEnumerator DoShake(Vector3 originalPosition, Quaternion originalRotation, float intensity)
        {
            while (intensity > 0)
            {
                transform.localPosition = originalPosition + Random.insideUnitSphere * intensity;
                transform.localRotation = new Quaternion(originalRotation.x + Random.Range(-intensity, intensity) * .2f,
                                                    originalRotation.y + Random.Range(-intensity, intensity) * .2f,
                                                    originalRotation.z + Random.Range(-intensity, intensity) * .2f,
                                                    originalRotation.w + Random.Range(-intensity, intensity) * .2f);

                intensity -= shakeDecay;

                yield return new WaitForSeconds(0.01f);
            }
            isShaking = false;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }
}
