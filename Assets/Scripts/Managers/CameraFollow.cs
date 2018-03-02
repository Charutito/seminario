using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private bool autoFindPlayer = false;

        [SerializeField] private float moveSpeed = 3f;

        private void Start()
        {
            if (autoFindPlayer && target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player");
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * moveSpeed); ;
        }
    }
}

