using System;
using System.Collections;
using System.Collections.Generic;
using GameUtils;
using Managers.Mappings;
using UnityEngine;
using Util;
using XInputDotNetPure;

namespace Managers
{
    public class InputManager : SingletonObject<InputManager>
    {
        [SerializeField] private InputMapping keyboard;
        [SerializeField] private InputMapping joystick;

        private static Coroutine _stopCoroutine;

        #region Properties
        
        public bool AxisMoving { get { return (Mathf.Abs(AxisHorizontal) + Mathf.Abs(AxisVertical)) > 0; } }
        
        public float AxisHorizontal { get { return Input.GetAxis("Horizontal"); } }
        public float AxisVertical { get { return Input.GetAxis("Vertical"); } }
        
        public bool Attack { get { return Input.GetKeyDown(keyboard.Attack) || Input.GetKeyDown(joystick.Attack); } }
        public bool SpecialAttack { get { return Input.GetKeyDown(keyboard.SpecialAttack) || Input.GetKeyDown(joystick.SpecialAttack); } }
        public bool CastGratiton { get { return Input.GetKeyDown(keyboard.Graviton) || Input.GetKeyDown(joystick.Graviton); } }
        
        public bool FirstAbility { get { return Input.GetKeyDown(keyboard.FirstAbility) || Input.GetKeyDown(joystick.FirstAbility); } }
        public bool SecondAbility { get { return Input.GetKeyDown(keyboard.SecondAbility) || Input.GetKeyDown(joystick.SecondAbility); } }

        public bool ThirdAbility { get { return Mathf.Abs(Input.GetAxis("SpellAim")) > 0; } }
        public bool FourthAbility { get { return Mathf.Abs(Input.GetAxis("SpellCast")) > 0; } }
        
        public bool Dash { get { return Input.GetKeyDown(keyboard.Dash) || Input.GetKeyDown(joystick.Dash); } }
        
        #endregion

        public void Vibrate(float left, float right, float duration = 0.1f)
        {
            GamePad.SetVibration(0, left, right);

            if (_stopCoroutine != null)
            {
                StopCoroutine(_stopCoroutine);
            }

            _stopCoroutine = StartCoroutine(StopVibration(duration));
        }
        
        private IEnumerator StopVibration(float duration)
        {
            yield return new WaitForSeconds(duration);
            
            GamePad.SetVibration(0, 0, 0);
        }

        private void OnDestroy()
        {
            GamePad.SetVibration(0, 0, 0);
        }
    }
}
