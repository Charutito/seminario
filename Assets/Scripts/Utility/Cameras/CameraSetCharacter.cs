using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Managers;
using UnityEngine;

namespace Utility
{
    public class CameraSetCharacter : MonoBehaviour
    {
        public bool SetFollow = true;
        public bool SetLookAt = false;
        
        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            if (SetFollow) _camera.Follow = GameManager.Instance.Character.transform;
            if (SetLookAt) _camera.LookAt = GameManager.Instance.Character.transform;
        }
    }
}
