using Managers;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Speed = 10;
    public float SpeedBoostMultiplier = 1.5f;
    
    protected Vector3 Direction;

    protected void Start()
    {
        Direction = transform.forward;
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
    }
}
