using UnityEngine;
using BattleSystem;
using BattleSystem.Spells;
using Entities;

public class CrossCollisiondmg : MonoBehaviour
{
    public int Damage;
    public GameObject Part;
    public DamageType Type = DamageType.Laser;
    public BuffEffect LaserDebuff;
    
    /*private void OnTriggerEnter(Collider col)
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
    }*/

    private void OnTriggerStay(Collider other)
    {
        var entity = other.GetComponent<Entity>();

        if (entity != null)
        {
            LaserDebuff.ApplyTo(entity);
        }
    }
}

