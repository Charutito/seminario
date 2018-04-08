using UnityEngine;

namespace BattleSystem.Spells
{
	public class BulletMove : MonoBehaviour
	{
		public float speed = 5f;

		private void Update()
		{
			transform.localPosition += transform.forward * speed * Time.deltaTime;
		}
	}
}
