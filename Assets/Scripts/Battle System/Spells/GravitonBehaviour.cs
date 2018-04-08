using System.Collections.Generic;
using Entities;
using UnityEngine;
using Util;

namespace BattleSystem.Spells
{
	public class GravitonBehaviour : MonoBehaviour
	{
		private void PullToCenter()
		{
			var colliders = Physics.OverlapSphere(transform.position, transform.localScale.x);
			
			foreach (var target in colliders)
			{
				if (target.CompareTag("Player")) return;

				var entity = target.GetComponent<Entity>();

				if (entity != null)
				{
					entity.Agent.ResetPath();
					entity.EntityMove.SmoothMoveTransform(transform.position, 0.5f);
				}
			}
		}
		private void Start()
		{
			PullToCenter();
		}
	}
}
