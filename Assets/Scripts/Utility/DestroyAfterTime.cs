using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
	public class DestroyAfterTime : MonoBehaviour
	{
		public float lifeTime = 2f;

		public GameObject deathEffect;
	
		private void Start()
		{
			Destroy(gameObject, lifeTime);
		}

		private void OnDestroy()
		{
			if(deathEffect != null) Instantiate(deathEffect, transform.position, transform.rotation);
		}
	}
}