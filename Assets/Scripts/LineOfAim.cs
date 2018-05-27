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

        return targets.Select(target => target.collider.GetComponent<Entity>()).Where(entity => entity != null).ToList();
    }

    private void RotateInstant(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
    }

    private void Start()
    {
        //if (!HitCollider)
        //{
        //    HitCollider = GetComponentInChildren<Collider>();
        //}

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
                var characterPos = GameManager.Instance.Character.transform;
                RotateInstant(characterPos.position + characterPos.forward);
            }
        }
	}
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
            
        Gizmos.DrawRay(HitCollider.transform.position, HitCollider.transform.forward);
        Gizmos.DrawWireCube(HitCollider.transform.position, HitCollider.transform.localScale);
    }
}
