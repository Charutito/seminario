using Managers;
using UnityEngine;
using Utility;

public class BulletMove : MonoBehaviour
{
    public int ReturnDamage;
    public GameObject ReturnParticles;
    public float Speed = 10;
    public float SpeedBoostMultiplier = 1.5f;

    protected Vector3 Direction;

    protected BulletCollisionDamage DamageCollision;
    protected DestroyAfterSeconds BulletDeath;

    protected void Start()
    {
        Direction = transform.forward;
        DamageCollision = GetComponent<BulletCollisionDamage>();
        BulletDeath = GetComponent<DestroyAfterSeconds>();
    }

    protected void Update()
    {
        transform.localPosition += Direction * Speed * Time.deltaTime;
    }

    public virtual void TakeDamage()
    {
        gameObject.layer = GameManager.Instance.Character.gameObject.layer;
        Speed *= SpeedBoostMultiplier;
        Direction = -Direction;
        DamageCollision.Damage = ReturnDamage;
        BulletDeath.DeathEffect = ReturnParticles;
    }
}
