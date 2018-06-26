using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EndDoor : MonoBehaviour {
    public ControlConsole[] consoles;
    public GameObject[] lasers;
    public NavMeshObstacle collider;
    int lifes;

	void Awake () {
        lifes = lasers.Length-1;
        foreach (var item in consoles)
        {
            item.OnConsoleDestroy += LifeLoss;
        }
	}

    void LifeLoss()
    {
        if (lasers[lifes]!=null)
        {
            Destroy(lasers[lifes]);
        }
        lifes--;

        if (lifes <= 0)
        {
            Debug.LogError(lifes);
            collider.enabled = false;
        }
    }
}
