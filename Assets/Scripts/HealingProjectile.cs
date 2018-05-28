using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class HealingProjectile : MonoBehaviour {

    Transform target;
    public float speed;
    private void Start()
    {
        target = GameManager.Instance.Character.transform;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (transform.position == target.transform.position)
        {
            Destroy(this.gameObject);
        }
    }


    
}
