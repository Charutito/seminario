using UnityEngine;
using UnityEngine.AI;

namespace Util
{
    public static class Utility
    {
        public static Vector3 RandomDirection()
        {
            var theta = Random.Range(0f, 2f * Mathf.PI);
            var phi = Random.Range(0f, Mathf.PI);
            var u = Mathf.Cos(phi);
            return new Vector3(Mathf.Sqrt(1 - u * u) * Mathf.Cos(theta), Mathf.Sqrt(1 - u * u) * Mathf.Sin(theta), u);
        }
        
        public static float GetMaxDistance(this Transform trans, float distance = float.MaxValue)
        {
            return trans.GetMaxDistance(trans.forward, distance);
        }

        public static float GetMaxDistance(this Transform trans, Vector3 forward, float distance = float.MaxValue)
        {
            var targetPos = trans.position + forward * distance;
            NavMeshHit hit;
            return NavMesh.Raycast(trans.position, targetPos, out hit, NavMesh.AllAreas) ? hit.distance : distance;
        }
        
        public static Vector3 GetMaxDistancePosition(this Transform trans, Vector3 forward, float distance = float.MaxValue)
        {
            var targetPos = trans.position + forward * distance;
            NavMeshHit hit;
            return NavMesh.Raycast(trans.position, targetPos, out hit, NavMesh.AllAreas) ? hit.position : targetPos;
        }
    }
}
