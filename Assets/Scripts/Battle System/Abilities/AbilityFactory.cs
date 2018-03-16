using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Abilities
{
    [CreateAssetMenu(menuName = "Battle System/Abilities/New Factory")]
    public class AbilityFactory : ScriptableObject
    {
        public List<AbilityDefinition> Definitions { get { return definitions; } }

        [SerializeField]
        private List<AbilityDefinition> definitions;
    }
}