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
	    public bool IsNegative;
        public float TimeToMove = 0.5f;
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
					entity.Agent.ResetPath();
					entity.EntityMove.SmoothMoveTransform(transform.position, TimeToMove);
				}
			}
		}

	    private void CastNegative(IEnumerable<Collider> colliders)
		{
            entidades = colliders.Select(x => x.GetComponent<Entity>()).Where(x => x != null).ToArray();
            SetArroundPoint(entidades, _behaviour.Definition.EffectRadius, transform);
            SetArroundPoint(entidades, _behaviour.Definition.EffectRadius + Displacement, transform);
        }

		private void SetArroundPoint(IList<Entity> objects, float radius, Transform center)
        {
            for (var i = 0; i < objects.Count; i++)
            {
                var a = i * 360/ objects.Count;
                var pos = RandomCircle(center.position, radius, a);
                objects[i].EntityMove.SmoothMoveTransform(pos, TimeToMove);
            }
        }
	        
        Vector3 RandomCircle(Vector3 center, float radius, int a)
        {
            float ang = a;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
            return pos;
        }
    }
}
