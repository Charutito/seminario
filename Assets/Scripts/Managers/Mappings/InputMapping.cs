using System;
using System.Collections;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;

namespace Managers.Mappings
{
    [CreateAssetMenu(menuName = "Game/Input/Mapping")]
    [Serializable]
    public class InputMapping : ScriptableObject
    {
        [SerializeField] public string MapName           = "Generic Mapping";

        [Header("Combat")]
        [SerializeField] public KeyCode Attack           = KeyCode.Mouse0;
        [SerializeField] public KeyCode SpecialAttack    = KeyCode.Mouse1;
        [SerializeField] public KeyCode ChargedAttack    = KeyCode.LeftControl;
        
        [Header("Spells")]
        [SerializeField] public KeyCode FirstAbility     = KeyCode.Alpha1;
        [SerializeField] public KeyCode SecondAbility    = KeyCode.Alpha2;

        [Header("Inventory")]
        [SerializeField] public KeyCode SelectWeapon1    = KeyCode.None;
        [SerializeField] public KeyCode SelectWeapon2    = KeyCode.None;
        [SerializeField] public KeyCode SelectWeapon3    = KeyCode.None;
        [SerializeField] public KeyCode SelectWeapon4    = KeyCode.None;
        [SerializeField] public KeyCode LastWeapon       = KeyCode.Q;
        [SerializeField] public KeyCode NextWeapon       = KeyCode.E;

        [Header("Movement")]
        public KeyCode Dash = KeyCode.Space;
    }
}
