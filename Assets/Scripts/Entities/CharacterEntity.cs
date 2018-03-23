using BattleSystem;
using FSM;
using Managers;
using System;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        private CharacterFSM fsm;

        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnHeavyAttack = delegate { };

        protected override void OnUpdate() 
        {
            fsm.Update();

            if (InputManager.Instance.AxisMoving && OnMove != null)
            {
                OnMove();
            }

            if (InputManager.Instance.LightAttack && OnAttack != null)
            {
                OnAttack();
            }

            if (InputManager.Instance.HeavyAttack && OnHeavyAttack != null)
            {
                OnHeavyAttack();
            }
        }

        private void Start()
        {
            fsm = new CharacterFSM(this);
        }
    }
}
