using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [MinMaxRange(0, 10)]
    public RangedFloat ShieldRange;
    
    public GameObject Shield;
        
    public Entity Entity;

    public void Deactivate(float delay = 0.5f)
    {
        iTween.ScaleTo(Shield, iTween.Hash("scale", new Vector3(ShieldRange.minValue, ShieldRange.minValue, ShieldRange.minValue), "time", delay, "easeType", iTween.EaseType.easeInSine));
    }
    
    public void Activate(float delay = 0.5f)
    {
        if (Entity != null)
        {
            Entity.IsInvulnerable = true;
        }
        
        iTween.ScaleTo(Shield, iTween.Hash("scale", new Vector3(ShieldRange.maxValue, ShieldRange.maxValue, ShieldRange.maxValue), "time", delay, "easeType", iTween.EaseType.easeInSine));
    }

    private void Awake()
    {
        Entity = Entity ?? GetComponentInParent<Entity>();
    }

    private void Start()
    {
        Activate();
    }

    private void OnDestroy()
    {
        if (Entity != null)
        {
            Entity.IsInvulnerable = false;
        }
    }
}
