using System;
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
					DoDamage(entity);
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
				    var effectRadious = Vector3.Distance(entity.transform.position, transform.position) - _behaviour.Definition.EffectRadius;
				    var movement = Vector3.MoveTowards(entity.transform.position, transform.position, effectRadious);
				    Action onMoveEnd = () => SpellDefinition.CastChild(_behaviour.Definition,
					    entity.transform.position + entity.transform.up * 3, Quaternion.identity, entity.transform);
				    
				    entity.EntityMove.SmoothMoveTransform(movement, TimeToMove, onMoveEnd);
				    DoDamage(entity);
			    }
		    }
	    }

	    private void DoDamage(IDamageable damageable)
	    {
		    damageable.TakeDamage(new Damage
		    {
			    amount = _behaviour.Definition.Damage,
			    type = _behaviour.Definition.DamageType,
			    origin = transform
		    });
	    }
    }
}
