using System.Runtime.Serialization.Formatters;
using Managers;
using UnityEngine;

namespace BattleSystem.Spells
{
	public class GravitonCaster : MonoBehaviour
	{
		public GameObject PositiveGraviton;
		public GameObject NegativeGraviton;
        public bool type;
        public void cast()
		{
			if (!type)
			{
                var spell = Instantiate(NegativeGraviton, transform);
                Destroy(spell.gameObject,3);
                Destroy(gameObject);
			}
			else if (type)
            {
                var spell = Instantiate(PositiveGraviton, transform);
                Destroy(spell.gameObject, 3);
                Destroy(gameObject);
			}
		}
	}
}
