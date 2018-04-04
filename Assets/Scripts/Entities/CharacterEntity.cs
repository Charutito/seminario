using FSM;
using Managers;
using System;
using BattleSystem;
using UnityEngine;

namespace Entities
{
    public class CharacterEntity : Entity
    {
	    [Header("Dash")]
	    public int dashLenght = 2;
	    public int maxDashCharges = 3;
	    public int currentDashCharges = 3;
	    public float dashChargesCooldown = 4f;
	    public float currentDashCooldown = 0;
	    
	    
	    
        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnDash = delegate { };
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

	    protected override void SetFsm()
	    {
		    currentDashCharges = maxDashCharges;
		    EntityFsm = new CharacterFSM(this);
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
		    if (InputManager.Instance.Dash && OnDash != null)
		    {
			    OnDash();
		    }
		    if (InputManager.Instance.SpecialAttack && OnSpecialAttack != null)
		    {
			    OnSpecialAttack();
		    }
		    if (InputManager.Instance.ChargedAttackDown && OnChargedAttack != null)
		    {
			    OnChargedAttack();
		    }

		    if (currentDashCharges < maxDashCharges)
		    {
			    if (currentDashCooldown <= 0)
			    {
				    currentDashCharges++;

				    currentDashCooldown = dashChargesCooldown;
			    }
			    
			    currentDashCooldown -= Time.deltaTime;
		    }
		    else
		    {
			    currentDashCooldown = dashChargesCooldown;
		    }

		    base.Update();
	    }
    }
}
