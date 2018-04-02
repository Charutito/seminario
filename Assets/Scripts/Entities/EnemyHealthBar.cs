using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
	public class EnemyHealthBar : MonoBehaviour
	{
		[SerializeField] private Image fillBar;
		
		private Entity entity;
		
		protected virtual void OnActualChange(float old, float current)
		{
			fillBar.fillAmount = current/entity.Stats.Health.Max;
		}

		protected virtual void OnDeath(Entity e)
		{
			Destroy(gameObject);
		}

		private void Awake()
		{
			entity = GetComponentInParent<Entity>();
			
			entity.Stats.Health.OnActualChange += OnActualChange;
			entity.OnDeath += OnDeath;
		}

		private void OnDestroy()
		{
			entity.OnDeath -= OnDeath;
		}
	}
}