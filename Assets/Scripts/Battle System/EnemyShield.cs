using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [MinMaxRange(0, 10)]
    public RangedFloat ShieldRange;
    
    public GameObject Shield;
    
    public LineRenderer Connection;

    public GameObject HitEffect;

    public Entity Entity;

    [HideInInspector]
    public ShieldGeneratorEntity Generator;

    public void Deactivate(float delay = 0.5f)
    {
        if (Connection != null)
        {
            Connection.gameObject.SetActive(false);
        }

        if (Shield != null)
        {
            iTween.ScaleTo(Shield, iTween.Hash("scale", new Vector3(ShieldRange.minValue, ShieldRange.minValue, ShieldRange.minValue), "time", delay, "easeType", iTween.EaseType.easeInSine));
        }
    }
    
    public void Activate(float delay = 0.5f)
    {
        if (Entity != null)
        {
            Entity.IsInvulnerable = true;
        }
        
        Connection.gameObject.SetActive(true);
        iTween.ScaleTo(Shield, iTween.Hash("scale", new Vector3(ShieldRange.maxValue, ShieldRange.maxValue, ShieldRange.maxValue), "time", delay, "easeType", iTween.EaseType.easeInSine));
    }

    private void Awake()
    {
        Entity = Entity ?? GetComponentInParent<Entity>();

        Entity.OnDeath += HandleDestroy;
    }

    private void Start()
    {
        Activate();
    }

    private void LateUpdate()
    {
        if (Generator == null) return;
        
        Connection.SetPosition(0, transform.position + Vector3.up);
        Connection.SetPosition(1, Generator.transform.position + Vector3.up);
    }

    private void HandleDestroy(Entity entity)
    {
        Deactivate();
    }

    private void OnDestroy()
    {
        if (Entity != null)
        {
            Entity.IsInvulnerable = false;
        }
    }
}
