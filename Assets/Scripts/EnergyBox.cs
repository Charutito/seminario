using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class EnergyBox : Collectable {
    public float HealingPow;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<CharacterEntity>();
        player.HealEnergy(HealingPow);
        Destroy(gameObject);
    }
}
