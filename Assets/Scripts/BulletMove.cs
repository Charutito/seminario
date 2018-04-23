using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed = 10;
    private int _direction = 1;
		
    private void Update()
    {
        transform.localPosition += transform.forward * _direction * Speed * Time.deltaTime;
    }

    public void ChangeDir()
    {
        _direction = -_direction;
    }
}
