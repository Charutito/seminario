using System;
using System.Collections.Generic;
using GameUtils;
using Managers.Mappings;
using UnityEngine;

namespace Managers
{
    public class InputManager : SingletonObject<InputManager>
    {
        [SerializeField] private InputMapping keyboard;
        [SerializeField] private InputMapping joystick;

        #region Properties
        
        public bool AxisMoving { get { return (Mathf.Abs(AxisHorizontal) + Mathf.Abs(AxisVertical)) > 0; } }
        
        public float AxisHorizontal { get { return Input.GetAxis("Horizontal"); } }
        public float AxisVertical { get { return Input.GetAxis("Vertical"); } }
        
        public bool Attack { get { return Input.GetKeyDown(keyboard.Attack) || Input.GetKeyDown(joystick.Attack); } }
        public bool SpecialAttack { get { return Input.GetKeyDown(keyboard.SpecialAttack) || Input.GetKeyDown(joystick.SpecialAttack); } }
        public bool ChargedAttackDown { get { return Input.GetKeyDown(keyboard.ChargedAttack) || Input.GetKeyDown(joystick.ChargedAttack); } }
        public bool ChargedAttackUp { get { return Input.GetKeyUp(keyboard.ChargedAttack) || Input.GetKeyUp(joystick.ChargedAttack); } }
        
        public bool FirstAbility { get { return Input.GetKeyDown(keyboard.FirstAbility) || Input.GetKeyDown(joystick.FirstAbility); } }
        public bool SecondAbility { get { return Input.GetKeyDown(keyboard.SecondAbility) || Input.GetKeyDown(joystick.SecondAbility); } }
        public bool ThirdAbility { get { return Input.GetKeyDown(keyboard.ThirdAbility) || Input.GetKeyDown(joystick.ThirdAbility); } }
        public bool FourAbility { get { return Input.GetKeyDown(keyboard.FourAbility) || Input.GetKeyDown(joystick.FourAbility); } }
        
        public bool SelectWeapon1 { get { return Input.GetKeyDown(keyboard.SelectWeapon1) || Input.GetKeyDown(joystick.SelectWeapon1); } }
        public bool SelectWeapon2 { get { return Input.GetKeyDown(keyboard.SelectWeapon2) || Input.GetKeyDown(joystick.SelectWeapon2); } }
        public bool SelectWeapon3 { get { return Input.GetKeyDown(keyboard.SelectWeapon3) || Input.GetKeyDown(joystick.SelectWeapon3); } }
        public bool SelectWeapon4 { get { return Input.GetKeyDown(keyboard.SelectWeapon4) || Input.GetKeyDown(joystick.SelectWeapon4); } }
        public bool LastWeapon { get { return Input.GetKeyDown(keyboard.LastWeapon) || Input.GetKeyDown(joystick.LastWeapon); } }
        public bool NextWeapon { get { return Input.GetKeyDown(keyboard.NextWeapon) || Input.GetKeyDown(joystick.NextWeapon); } }
        
        public bool Dash { get { return Input.GetKeyDown(keyboard.Dash) || Input.GetKeyDown(joystick.Dash); } }
        
        #endregion
    }
}
