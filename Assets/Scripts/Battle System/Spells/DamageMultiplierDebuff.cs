using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Entities;
using UnityEngine;

public class DamageMultiplierDebuff : MonoBehaviour
{
    private Entity _entity;
    
    private void Awake()
    {
        _entity = GetComponentInParent<Entity>();
        
        _entity.OnTakeDamage += OnEntityDamage;
    }

    private void OnEntityDamage(int damage, DamageType type)
    {
        _entity.OnTakeDamage -= OnEntityDamage;
        _entity.TakeDamage(damage, DamageType.Spell);
        Destroy(this);
    }
}
