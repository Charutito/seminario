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

        public event Action OnAnimUnlock = delegate { };
        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnHeavyAttack = delegate { };

        public void AnimUnlock()
        {
            if (OnAnimUnlock != null)
            {
                OnAnimUnlock();
            }
        }

        private void Update()
        {
            if (InputManager.Instance.AxisMoving && OnMove != null)
            {
                OnMove();
            }
            if (InputManager.Instance.Attack && OnAttack != null)

            if (InputManager.Instance.Attack && Time.time > nextFire && OnAttack != null)
            {
                nextFire = Time.time + fireRate;
                OnAttack();
            }
            if (InputManager.Instance.SpecialAttack && OnHeavyAttack != null)
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
