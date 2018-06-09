using Managers;
using UnityEngine;

public class BulletBounceBehaviour : BulletMove
{
    public GameObject BounceParticle;
    
    public override void TakeDamage()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletBounce"))
        {
            Direction = Vector3.Reflect(Direction, -other.transform.right);
            Instantiate(BounceParticle, transform.position, transform.rotation);
        }
    }
}
