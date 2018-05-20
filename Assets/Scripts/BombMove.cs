using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMove : MonoBehaviour
{
    public float Speed = 10;
    public AnimationCurve ElevationOverDistance;
    public Vector3 TargetPosition;
    private float _initialDistance;

    private void Start()
    {
        _initialDistance = Vector3.Distance(transform.position, TargetPosition);
    }

    private void Update()
    {
        transform.localPosition = transform.up * ElevationOverDistance.Evaluate(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), TargetPosition) / _initialDistance);
        Debug.Log(Vector3.Distance(transform.position, TargetPosition) / _initialDistance);
        transform.localPosition += transform.forward  * Speed * Time.deltaTime;
    }
}
