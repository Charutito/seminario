using System;
using System.Collections.Generic;
using GameUtils;
using UnityEngine;

namespace Managers
{
    public class InputManager : SingletonObject<InputManager>
    {
        [Serializable]
        private class InputMap
        {
            public bool IsPressing { get { return Input.GetKeyDown(joystick) || Input.GetKeyDown(keyboard); } }

            [SerializeField] private KeyCode joystick = KeyCode.Joystick1Button0;
            [SerializeField] private KeyCode keyboard;
        }

        [SerializeField] private InputMap lightAttack;
        [SerializeField] private InputMap heavyAttack;

        public float AxisHorizontal
        {
            get { return Input.GetAxis("Horizontal"); }
        }

        public float AxisVertical
        {
            get { return Input.GetAxis("Vertical"); }
        }

        public bool AxisMoving
        {
            get { return (Mathf.Abs(AxisHorizontal) + Mathf.Abs(AxisVertical)) > 0; }
        }

        public bool LightAttack
        {
            get { return lightAttack.IsPressing; }
        }

        public bool HeavyAttack
        {
            get { return heavyAttack.IsPressing; }
        }
    }
}
