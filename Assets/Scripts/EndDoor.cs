using UnityEngine;
using UnityEngine.AI;

public class EndDoor : MonoBehaviour
{
    public ControlConsole[] consoles;
    public GameObject[] lasers;
    public NavMeshObstacle navmeshCollider;
    private int _lifes;

    private void Awake ()
	{
        _lifes = lasers.Length-1;
	    
        foreach (var item in consoles)
        {
            item.OnConsoleDestroy += LifeLoss;
        }
	}

    private void LifeLoss()
    {
        if (lasers[_lifes]!=null)
        {
            Destroy(lasers[_lifes]);
        }
        _lifes--;

        if (_lifes <= 0 && navmeshCollider != null)
        {
            navmeshCollider.enabled = false;
        }
    }
}
