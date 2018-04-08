using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace Entities.Base
{
	public class EntitySpells : MonoBehaviour
	{
		public void Cast(SpellDefinition definition, Transform pos)
		{
			Instantiate(definition.prefab, pos.position, transform.rotation);
		}
	}
}
