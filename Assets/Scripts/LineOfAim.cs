using GameUtils;
using Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class LineOfAim : MonoBehaviour
{
    private List<GameObject> lastEnemiesInSight;

    private ColliderObserver hitCollider;

    public void GetEnemiesInSight(Action<List<GameObject>> callback)
    {
        lastEnemiesInSight.Clear();
        hitCollider.gameObject.SetActive(true);

        FrameUtil.OnNextFrame(()=>
        {
            hitCollider.gameObject.SetActive(false);
            callback(lastEnemiesInSight);
        });
    }

    private void HitCollider_TriggerEnter(Collider obj)
    {
        lastEnemiesInSight.Add(obj.gameObject);
    }

    public void RotateInstant(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
    }

    private void Awake()
    {
        hitCollider = GetComponentInChildren<ColliderObserver>();
    }

    private void Start()
    {
        lastEnemiesInSight = new List<GameObject>();
        hitCollider.TriggerEnter += HitCollider_TriggerEnter;
        hitCollider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.AxisMoving)
        {
             RotateInstant(new Vector3(transform.position.x + InputManager.Instance.AxisHorizontal, transform.position.y, transform.position.z + InputManager.Instance.AxisVertical));
        }
	}
}
