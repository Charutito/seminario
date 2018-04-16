using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Managers.Camera
{
    public class CameraPoint : MonoBehaviour {
   
     BoxCollider TriggerArea;
     CameraFollow cameraScript;
     GameObject player;

        // Use this for initialization
        public void Awake()
        {
            TriggerArea = GetComponent<BoxCollider>();
            cameraScript = GameObject.FindObjectOfType<CameraFollow>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                player = cameraScript.target;
                cameraScript.target = this.gameObject;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            cameraScript.target = player;
            Debug.Log("hit2");

        }
    }
    }

