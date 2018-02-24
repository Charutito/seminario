using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class Utility
    {
        public static Vector3 RandomDirection()
        {
            var theta = Random.Range(0f, 2f * Mathf.PI);
            var phi = Random.Range(0f, Mathf.PI);
            var u = Mathf.Cos(phi);
            return new Vector3(Mathf.Sqrt(1 - u * u) * Mathf.Cos(theta), Mathf.Sqrt(1 - u * u) * Mathf.Sin(theta), u);
        }
    }
}
