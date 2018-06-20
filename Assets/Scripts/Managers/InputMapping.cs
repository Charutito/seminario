using System;
using System.Collections;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;

namespace Managers.Mappings
{
    [CreateAssetMenu(menuName = "Akane/Input/Mapping")]
    [Serializable]
    public class InputMapping : ScriptableObject
    {
        [SerializeField] public string MapName           = "Generic Mapping";

        [Header("Combat")]
        [SerializeField] public KeyCode Attack           = KeyCode.Mouse0;
        [SerializeField] public KeyCode SpecialAttack    = KeyCode.Mouse1;
        
        [Header("Spells")]
        [SerializeField] public KeyCode FirstAbility     = KeyCode.Alpha2;
        [SerializeField] public KeyCode SecondAbility    = KeyCode.Alpha3;
        [SerializeField] public KeyCode ThirdAbility    = KeyCode.Alpha1;
        [SerializeField] public KeyCode FourthAbility    = KeyCode.Alpha4;

        [Header("Movement")]
        public KeyCode Dash = KeyCode.Space;
    }
}
