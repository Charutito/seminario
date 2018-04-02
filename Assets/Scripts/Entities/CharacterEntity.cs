using FSM;
using Managers;
using System;
using BattleSystem;
using UnityEngine;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnStun = delegate { };
        public event Action OnSpecialAttack = delegate { };
        public event Action OnChargedAttack = delegate { };
	    
	    public override void TakeDamage(int damage, DamageType type)
	    {
		    if (type == DamageType.SpecialAttack)
		    {
			    OnStun();
			    
			    if (IsSpecialAttacking)
			    {
				    damage = 0;
			    }
		    }

		    base.TakeDamage(damage, type);
	    }

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
