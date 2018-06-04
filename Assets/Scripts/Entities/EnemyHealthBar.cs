using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
	public class EnemyHealthBar : MonoBehaviour
	{
		[SerializeField] private Image fillBar;
		
		private Entity _entity;
		
		protected virtual void OnActualChange(int old, int current)
		{
			fillBar.fillAmount = (float)current / _entity.Stats.MaxHealth;
		}

		protected virtual void OnDeath(Entity e)
		{
			_entity.Stats.OnHealthChange -= OnActualChange;
			_entity.OnDeath -= OnDeath;
			Destroy(gameObject);
		}

		private void Start()
		{
			_entity = GetComponentInParent<Entity>();
			
			_entity.Stats.OnHealthChange += OnActualChange;
			_entity.OnDeath += OnDeath;
		}
	}
}