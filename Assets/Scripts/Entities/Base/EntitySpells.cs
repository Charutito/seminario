using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace Entities.Base
{
	public class EntitySpells : MonoBehaviour
	{
		public void Cast(SpellDefinition definition)
		{
			Instantiate(definition.prefab, transform.position + transform.forward, transform.rotation);
		}
	}
}
