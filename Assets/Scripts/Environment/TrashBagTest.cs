using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class TrashBagTest : MonoBehaviour, IDamageable
{
    private ParticleSystem _particles;

    public void TakeDamage(Damage damage)
    {
        _particles.Emit(10);
    }

    private void Awake ()
    {
        _particles = GetComponent<ParticleSystem>();
	}
}
