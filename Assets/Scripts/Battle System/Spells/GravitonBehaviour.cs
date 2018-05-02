using System.Collections.Generic;
using Entities;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.Build.AssetBundle;
using Util;

namespace BattleSystem.Spells
{
    [RequireComponent(typeof(SpellBehaviour))]
	public class GravitonBehaviour : MonoBehaviour
    {
        public float TimeToMove = 0.5f;
	    public bool IsNegative;
	    public float Displacement;
		
	    private Entity[] entidades;
	    private SpellBehaviour _behaviour;

	    private void Start()
	    {
	        _behaviour = GetComponent<SpellBehaviour>();
	        
	        transform.localScale = new Vector3(_behaviour.Definition.EffectRadius, _behaviour.Definition.EffectRadius, _behaviour.Definition.EffectRadius);
		    
		    var colliders = Physics.OverlapSphere(transform.position, _behaviour.Definition.EffectRadius, _behaviour.Definition.EffectLayer);
		    
		    if(IsNegative)
			    CastNegative(colliders);
		    else
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
	    
	    private void CastNegative(IEnumerable<Collider> colliders)
	    {
		    foreach (var target in colliders)
		    {
			    var entity = target.GetComponent<Entity>();

			    if (entity != null)
			    {
				    entity.EntityMove.SmoothMoveTransform(transform.position - (entity.transform.forward * _behaviour.Definition.EffectRadius), TimeToMove, () => SpellDefinition.CastChild(_behaviour.Definition, entity.transform.position + entity.transform.up * 3, Quaternion.identity, entity.transform));
				    entity.TakeDamage(_behaviour.Definition.Damage, _behaviour.Definition.DamageType);
			    }
		    }
	    }
    }
}
