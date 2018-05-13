using System.Runtime.Serialization.Formatters;
using Managers;
using UnityEngine;

namespace BattleSystem.Spells
{
	public class GravitonCaster : MonoBehaviour
	{
		private SpellBehaviour _behaviour;

		private void Awake()
		{
			_behaviour = GetComponent<SpellBehaviour>();
		}

		public void Cast()
		{
			SpellDefinition.CastChild(_behaviour.Definition, transform.position, transform.rotation, transform);
			Destroy(gameObject);
		}
	}
}
