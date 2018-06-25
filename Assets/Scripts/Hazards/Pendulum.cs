using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float LerpTime;
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
    IEnumerator LerpPosition(Vector3 StartPos, Vector3 EndPos, float LerpTime)
    {
        float StartTime = Time.time;
        float EndTime = StartTime + LerpTime;

        while (Time.time < EndTime)
        {
            float timeProgressed = (Time.time - StartTime) / LerpTime;  // this will be 0 at the beginning and 1 at the end.
           // transform.position = Mathf.Lerp(StartPos, EndPos, timeProgressed);
            transform.position = Vector3.Lerp(StartPos, _current.position, timeProgressed);

            yield return new WaitForFixedUpdate();
        }

    }
    private void Update ()
    {
        if (_current != null)
        {
            if (Vector3.Distance(transform.position, _current.position) <= 0.1f)
            {
                GetCurrent();
            }

            StartCoroutine(LerpPosition(transform.position, _current.position, LerpTime));
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
