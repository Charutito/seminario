using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Util;

public class HealingProjectile : MonoBehaviour
{
    public float Speed = 8f;
    [Range(0, 3)]
    public float DestroyRange = 1f;
    [Range(0, 1)]
    public float StartDelay = 0.3f;
    
    private Transform _target;
    private bool _active;
    
    private void Start()
    {
        _target = GameManager.Instance.Character.transform;

        FrameUtil.AfterDelay(StartDelay, () => _active = true);
    }
    
    private void Update()
    {
        if (!_active) return;

        var fixedPosition = _target.position + Vector3.up;
        transform.position = Vector3.MoveTowards(transform.position, fixedPosition, Speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, fixedPosition) <= DestroyRange)
        {
            Destroy(gameObject);
        }
    }


    
}
