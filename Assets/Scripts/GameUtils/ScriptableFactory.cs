using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class ScriptableFactory<T> : ScriptableObject
    {
        public List<T> Definitions { get { return definitions; } }

        [SerializeField] private List<T> definitions;
    }
}
