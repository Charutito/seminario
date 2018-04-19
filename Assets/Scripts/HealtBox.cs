using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class HealtBox : Collectable {
    public float HealingPow;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<CharacterEntity>();
        player.Heal(HealingPow);
        Destroy(gameObject);
    }
}
