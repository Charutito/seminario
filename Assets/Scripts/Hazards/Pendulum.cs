using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float Time;
    public Transform[] positions;

    private Transform _current;
    private int _currentIndex;

    public void Start()
    {
        _current = positions.FirstOrDefault();      
    }

    private void GetCurrent()
    {
        _currentIndex++;
        _current = positions[_currentIndex % positions.Length];
    }

    private void Update ()
    {
        if (_current != null)
        {
            if (Vector3.Distance(transform.position, _current.position) <= 0.1f)
            {
                GetCurrent();
            }
            
            transform.position = Vector3.Lerp(transform.position, _current.position, Time);
        }
        else if (positions.Length > 0)
        {
            GetCurrent();
        }
        else
        {
            Debug.LogWarning("This pendulum has no positions");
            Destroy(gameObject);
        }
    }
}
