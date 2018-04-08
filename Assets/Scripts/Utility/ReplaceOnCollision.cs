using UnityEngine;

namespace Utility
{
	public class ReplaceOnCollision : MonoBehaviour
	{
		public GameObject Prefab;

		private bool _isDiying = false;
		
		private void OnCollisionEnter(Collision other)
		{
			if (_isDiying) return;

			_isDiying = true;
			Instantiate(Prefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (_isDiying) return;

			_isDiying = true;
			Instantiate(Prefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
