using FSM;
using Managers;
using System;
using UnityEngine;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnSpecialAttack = delegate { };
        public event Action OnChargedAttack = delegate { };

	    protected override void Update()
	    {
		    if (InputManager.Instance.AxisMoving && OnMove != null)
		    {
			    OnMove();
		    }
		    if (InputManager.Instance.Attack && OnAttack != null)
		    {
			    OnAttack();
		    }
		    if (InputManager.Instance.SpecialAttack && OnSpecialAttack != null)
		    {
			    OnSpecialAttack();
		    }
		    if (InputManager.Instance.ChargedAttackDown && OnChargedAttack != null)
		    {
			    OnChargedAttack();
		    }
		    
		    base.Update();
	    }

        private void Start()
        {
	        EntityFsm = new CharacterFSM(this);
        }
    }
}
