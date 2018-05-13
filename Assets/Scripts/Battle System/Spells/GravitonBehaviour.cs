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
        public float TimeToMovePositive = 0.5f;
        public float TimeToMoveNegative = 0.5f;
        public float Delay = 0.8f;
        public float StartDelay = 0.8f;
        public bool IsNegative;
	    public float Displacement;
		
	    private Entity[] entidades;
	    private SpellBehaviour _behaviour;

	    private void Start()
	    {
	        _behaviour = GetComponent<SpellBehaviour>();
	        
	        transform.localScale = new Vector3(_behaviour.Definition.EffectRadius, _behaviour.Definition.EffectRadius, _behaviour.Definition.EffectRadius);
		    
		    var colliders = Physics.OverlapSphere(transform.position, _behaviour.Definition.EffectRadius, _behaviour.Definition.EffectLayer);

            FrameUtil.AfterDelay(StartDelay, () =>
            {
                if (IsNegative)
                    CastNegative(colliders);
                else
                    CastPositive(colliders);
            });

		    
	    }
		
		private void CastPositive(IEnumerable<Collider> colliders)
		{
			foreach (var target in colliders)
			{
				var entity = target.GetComponent<Entity>();

				if (entity != null)
				{
					entity.EntityMove.SmoothMoveTransform(transform.position, TimeToMovePositive);
					DoDamage(entity, _behaviour.Definition.DamageType);
				}
			}
			
			FrameUtil.AfterDelay(Delay, () => SpellDefinition.CastChild(_behaviour.Definition, transform.position, transform.rotation));
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
				    
				    entity.EntityMove.SmoothMoveTransform(movement, TimeToMoveNegative);
				    DoDamage(entity, DamageType.ThirdAttack);
			    }
		    }
	    }

	    private void DoDamage(IDamageable damageable, DamageType damageType)
	    {
		    damageable.TakeDamage(new Damage
		    {
			    amount = _behaviour.Definition.Damage,
			    type = damageType,
			    origin = transform
		    });
	    }
    }
}
