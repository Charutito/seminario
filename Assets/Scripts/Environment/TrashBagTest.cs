using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleSystem;

public class TrashBagTest : MonoBehaviour, IDamageable
{
    private ParticleSystem _particles;

    public void TakeDamage(int damage, DamageType type = DamageType.Unknown)
    {
        _particles.Emit(10);
    }

    public Transform Target()
    {
        throw new System.NotImplementedException();
    }

    private void Awake ()
    {
        _particles = GetComponent<ParticleSystem>();
	}
}
