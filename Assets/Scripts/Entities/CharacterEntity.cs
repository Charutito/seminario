using FSM;
using Managers;
using System;
using UnityEngine;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        public float fireRate = 0.5f;

        private CharacterFSM fsm;
        private float nextFire = 0f;

        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnHeavyAttack = delegate { };

        private void Update() 
        {
            if (InputManager.Instance.AxisMoving && OnMove != null)
            {
                OnMove();
            }
            if (InputManager.Instance.LightAttack && OnAttack != null)

            if (InputManager.Instance.LightAttack && Time.time > nextFire && OnAttack != null)
            {
                nextFire = Time.time + fireRate;
                OnAttack();
            }
            if (InputManager.Instance.HeavyAttack && OnHeavyAttack != null)
            {
                OnHeavyAttack();
            }
           
            fsm.Update();
        }

        private void Start()
        {
            fsm = new CharacterFSM(this);
        }
    }
}
