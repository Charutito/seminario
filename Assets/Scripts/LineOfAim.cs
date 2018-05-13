using GameUtils;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;
using Util;

public class LineOfAim : MonoBehaviour
{
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private Collider _hitCollider;
    [SerializeField] private bool _move = true;

    public IEnumerable<Entity> GetEnemiesInSight()
    {
        var targets = Physics.BoxCastAll(_hitCollider.transform.position, _hitCollider.transform.localScale/2, transform.forward, _hitCollider.transform.rotation, 1, _hitLayers);

        return targets.Select(target => target.collider.GetComponent<Entity>()).Where(entity => entity != null).ToList();
    }

    private void RotateInstant(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
    }

    private void Start()
    {
        _hitCollider = _hitCollider ?? GetComponentInChildren<Collider>();
    }

    private void Update()
    {
        if (_move && InputManager.Instance.AxisMoving)
        {
             RotateInstant(new Vector3(transform.position.x + InputManager.Instance.AxisHorizontal, transform.position.y, transform.position.z + InputManager.Instance.AxisVertical));
        }
	}
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
            
        Gizmos.DrawRay(_hitCollider.transform.position, _hitCollider.transform.forward);
        Gizmos.DrawWireCube(_hitCollider.transform.position, _hitCollider.transform.localScale);
    }
}
