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
    public Collider HitCollider;
    
    [SerializeField] private LayerMask _hitLayers;
    [SerializeField] private bool _move = true;

    public IEnumerable<Entity> GetEnemiesInSight()
    {
        var targets = Physics.BoxCastAll(HitCollider.transform.position, HitCollider.transform.localScale/2, transform.forward, HitCollider.transform.rotation, 1, _hitLayers);

        return targets
            .Select(target => target.collider.GetComponent<Entity>())
            .Where(entity => entity != null)
            .ToList();
    }
    
    public Entity GetFirstEnemyInSight()
    {
        var targets = Physics.BoxCastAll(HitCollider.transform.position, HitCollider.transform.localScale/2, transform.forward, HitCollider.transform.rotation, 1, _hitLayers);

        return targets
            .Select(target => target.collider.GetComponent<Entity>())
            .FirstOrDefault(entity => entity != null);
    }

    private void RotateInstant(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
    }

    private void Start()
    {
        HitCollider = HitCollider ? HitCollider : GetComponentInChildren<Collider>();
    }

    private void Update()
    {
        if (_move)
        {
            if (InputManager.Instance.AxisMoving)
            {
                RotateInstant(new Vector3(transform.position.x + InputManager.Instance.AxisHorizontal, transform.position.y, transform.position.z + InputManager.Instance.AxisVertical));
            }
            else
            {
                var entity = GetFirstEnemyInSight();
                
                if (entity != null)
                {
                    RotateInstant(entity.transform.position);
                }
                else
                {
                    transform.localRotation = Quaternion.identity;
                }
            }
        }
	}
}
