using System.Collections.Generic;
using Entities;
using UnityEngine;
using System.Linq;
using Util;

namespace BattleSystem.Spells
{
	public class GravitonBehaviour : MonoBehaviour
	{
        float TimeToMove = 0.5f;
        public float radius;
        public float displacement;
        public Entity[] entidades;

        private void Start()
		{
            transform.localScale = new Vector3(radius, radius, radius);
            var colliders = Physics.OverlapSphere(transform.position, radius);
            entidades = colliders.Select(x => x.GetComponent<Entity>()).Where(x => x!=null).ToArray();
            SetArroundPoint(entidades, radius, transform);
            SetArroundPoint(entidades, radius + displacement, transform);
        }

        public void SetArroundPoint(Entity[] objects, float radius, Transform center)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                int a = i * 360/ objects.Length;
                Vector3 pos = RandomCircle(center.position, radius, a);
                objects[i].EntityMove.SmoothMoveTransform(pos, TimeToMove);
            }
        }
        Vector3 RandomCircle(Vector3 center, float radius, int a)
        {
            Debug.Log(a);
            float ang = a;
            Vector3 pos;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
            return pos;
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius+displacement);
        }
    }
}
