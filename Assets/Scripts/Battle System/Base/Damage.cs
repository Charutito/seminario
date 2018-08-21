using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
	[Serializable]
	public struct Damage
	{
		public int Amount;
		public DamageType Type;
		public Vector3 OriginPosition;
		public Quaternion OriginRotation;
		public float Displacement;
		public bool Absolute;
	}
}
