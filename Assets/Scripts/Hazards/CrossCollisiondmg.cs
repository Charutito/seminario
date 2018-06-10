using UnityEngine;
using BattleSystem;

public class CrossCollisiondmg : MonoBehaviour
{
    public int Damage;
    public GameObject Part;
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
            var Fx = Instantiate(Part);
            Fx.transform.position = col.transform.position;
            Destroy(Fx, 1f);
        }
    }
}

