using System.Runtime.Serialization.Formatters;
using Managers;
using UnityEngine;

namespace BattleSystem.Spells
{
	public class GravitonCaster : MonoBehaviour
	{
		public SpellDefinition PositiveGraviton;
		public SpellDefinition NegativeGraviton;

		private void Update()
		{
			if (InputManager.Instance.FirstAbility)
			{
				SpellDefinition.Cast(NegativeGraviton, transform);
				Destroy(gameObject);
			}
			else if (InputManager.Instance.SecondAbility)
			{
				SpellDefinition.Cast(PositiveGraviton, transform);
				Destroy(gameObject);
			}
		}
	}
}
