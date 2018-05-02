using System.Collections.Generic;
using Entities;
using UnityEngine;
using System.Linq;
using Util;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
	public class GravitonBehaviour : MonoBehaviour
    {
        public float TimeToMove = 0.5f;
	    private SpellBehaviour _behaviour;

	    private void Start()
	    {
	        _behaviour = GetComponent<SpellBehaviour>();
	        
	        transform.localScale = new Vector3(_behaviour.Definition.EffectRadius, _behaviour.Definition.EffectRadius, _behaviour.Definition.EffectRadius);
		    
		    var colliders = Physics.OverlapSphere(transform.position, _behaviour.Definition.EffectRadius, _behaviour.Definition.EffectLayer);
		    
		    CastPositive(colliders);
	    }
		
		private void CastPositive(IEnumerable<Collider> colliders)
		{
			foreach (var target in colliders)
			{
				var entity = target.GetComponent<Entity>();

				if (entity != null)
				{
					entity.EntityMove.SmoothMoveTransform(transform.position, TimeToMove);
					entity.TakeDamage(_behaviour.Definition.Damage, _behaviour.Definition.DamageType);
				}
			}
		}
    }
}
