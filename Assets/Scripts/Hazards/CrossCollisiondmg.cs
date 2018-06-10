using UnityEngine;
using BattleSystem;

public class CrossCollisiondmg : MonoBehaviour
{
    public int Damage;
    
    private void OnTriggerEnter(Collider col)
    {
        var damageable = col.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.TakeDamage(new Damage
            {
                Amount = Damage,
                Type = DamageType.Attack,
                Origin = transform
            });
        }
    }
}

