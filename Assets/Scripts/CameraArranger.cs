using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Managers.Camera
{
    public class CameraArranger : MonoBehaviour {
    public float Range;
	private GameObject[] CameraPoints;
    private GameObject player;
    private CameraFollow cameraScript;
	void Awake () {
        CameraPoints = new GameObject[GameObject.FindGameObjectsWithTag("CameraPoint").Length];
        CameraPoints = GameObject.FindGameObjectsWithTag("CameraPoint");
        cameraScript = gameObject.GetComponent<CameraFollow>();          
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
    void Update () {
            if (CameraPoints.Any(x => Vector3.Distance(player.transform.position, x.transform.position) <= x.GetComponent<CameraPoint>().Range))
            {
                cameraScript.target = CameraPoints.First(x => Vector3.Distance(transform.position, x.transform.position) <= Range);
            }
            else if (!CameraPoints.Any(x => Vector3.Distance(player.transform.position, x.transform.position) <= Range))
            {
                cameraScript.target = player;
            }
	}
}
}

