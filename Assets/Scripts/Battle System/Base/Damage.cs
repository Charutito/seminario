using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
	[Serializable]
	sealed public class Damage
	{
		public int Amount;
		public DamageType Type = DamageType.Unknown;
		public Vector3 OriginPosition;
		public Quaternion OriginRotation;
		public float Displacement;
		public bool Absolute;
	}
}
