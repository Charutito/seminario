using System.Collections;
using System.Collections.Generic;
using Managers;
using Metadata;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed = 10;
    public float SpeedBoostMultiplier = 1.5f;
    private int _direction = 1;

    private void Update()
    {
        transform.localPosition += transform.forward * _direction * Speed * Time.deltaTime;
    }

    public void ChangeDir()
    {
        gameObject.layer = GameManager.Instance.Character.gameObject.layer;
        Speed *= SpeedBoostMultiplier;
        _direction = -_direction;
    }
}
