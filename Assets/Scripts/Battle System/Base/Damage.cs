using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
	[Serializable]
	sealed public class Damage
	{
		[SerializeField] public int amount;
		[SerializeField] public DamageType type = DamageType.Unknown;
		[SerializeField] public Transform origin;
		[SerializeField] public Entity originator;
		[SerializeField] public float Displacement;
		[SerializeField] public bool Absolute;
	}
}
