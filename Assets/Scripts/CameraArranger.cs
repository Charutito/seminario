using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Managers.Camera
{
    public class CameraArranger : MonoBehaviour {
    public float Range;
	public GameObject[] CameraPoints;
    public GameObject player;
    public CameraFollow cameraScript;
	void Awake () {
        CameraPoints = new GameObject[GameObject.FindGameObjectsWithTag("CameraPoint").Length];
        CameraPoints = GameObject.FindGameObjectsWithTag("CameraPoint");
        cameraScript = gameObject.GetComponent<CameraFollow>();
        
        //entity.NextPos = entity.PosToFlee.First(x => x != entity.NextPos);

    }
    // Update is called once per frame
    void Update () {
            if(CameraPoints.Any(x => Vector3.Distance(player.transform.position, x.transform.position) <= Range))
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

