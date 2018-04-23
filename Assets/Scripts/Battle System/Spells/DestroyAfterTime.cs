using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Spells
{
	[RequireComponent(typeof(SpellBehaviour))]
	public class DestroyAfterTime : MonoBehaviour
	{
		private SpellBehaviour _behaviour;
		private float _currentTimeToDestroy;
        
		private void Start()
		{
			_behaviour = GetComponent<SpellBehaviour>();
			_currentTimeToDestroy = _behaviour.Definition.DestroyAfterTime;

			if (_currentTimeToDestroy <= 0)
			{
				Destroy(this);
			}
		}

		private void Update()
		{
			_currentTimeToDestroy -= Time.deltaTime;
			
			if (_currentTimeToDestroy <= 0) SelfKill();
		}

		private void SelfKill()
		{
			if (_behaviour.Definition.DeathEffect != null)
			{
				Instantiate(_behaviour.Definition.DeathEffect, transform.position, transform.rotation);
			}

			Destroy(gameObject);
		}
	}
}