using UnityEngine;
using BattleSystem;

public class CrossCollisiondmg : MonoBehaviour
{
    public int Damage;
    public GameObject Part;
    public DamageType Type = DamageType.Laser;
    
    private void OnTriggerEnter(Collider col)
    {
        var damageable = col.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.TakeDamage(new Damage
            {
                Amount = Damage,
                Type = DamageType.Laser,
                OriginPosition = transform.position,
                OriginRotation = transform.rotation
            });
            
            var fx = Instantiate(Part);
            fx.transform.position = col.transform.position;
            Destroy(fx, 1f);
        }
    }
}

