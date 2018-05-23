using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Utility
{
    public class TriggerCameraCinemachine : MonoBehaviour
    {
        public CinemachineVirtualCamera Camera;

        private void ToggleCamera(bool state)
        {
            Camera.gameObject.SetActive(state);
        }

        private void OnTriggerEnter(Collider other)
        {
            ToggleCamera(true);
        }

        private void OnTriggerExit(Collider other)
        {
            ToggleCamera(false);
        }
    }
}
