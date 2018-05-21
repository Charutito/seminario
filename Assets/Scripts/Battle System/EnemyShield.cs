using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [MinMaxRange(0, 10)]
    public RangedFloat ShieldRange;
    
    public GameObject Shield;
        
    private Entity _entity;

    public void Deactivate(float delay = 0.5f)
    {
        iTween.ScaleTo(Shield, iTween.Hash("scale", new Vector3(ShieldRange.minValue, ShieldRange.minValue, ShieldRange.minValue), "time", delay, "easeType", iTween.EaseType.easeInSine));
    }
    
    public void Activate(float delay = 0.5f)
    {
        if (_entity != null)
        {
            _entity.IsInvulnerable = true;
        }
        
        iTween.ScaleTo(Shield, iTween.Hash("scale", new Vector3(ShieldRange.maxValue, ShieldRange.maxValue, ShieldRange.maxValue), "time", delay, "easeType", iTween.EaseType.easeInSine));
    }

    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
    }

    private void Start()
    {
        Activate();
    }

    private void OnDestroy()
    {
        if (_entity != null)
        {
            _entity.IsInvulnerable = false;
        }
    }
}
