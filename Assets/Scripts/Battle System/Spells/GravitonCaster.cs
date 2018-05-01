using System.Runtime.Serialization.Formatters;
using Managers;
using UnityEngine;

namespace BattleSystem.Spells
{
	public class GravitonCaster : MonoBehaviour
	{
		public GameObject NegativeGraviton;
        public bool type;
        public void cast()
		{
			if (!type)
			{
                var spell = Instantiate(NegativeGraviton);
                spell.transform.position = transform.position;
                Destroy(gameObject);
			}			
        }
	}
}
