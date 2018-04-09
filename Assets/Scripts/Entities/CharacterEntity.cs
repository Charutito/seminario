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
	    
        [Header("GetHit")]
        public float getHitDuration = 0.7f;
        [Range(0,1)]
        public float AtkDispl= 0.5f;
        [Range(0, 1)]
        public float DmgDispl = 0.5f;

        [Header("Spells")]
        public Transform castPosition;
	    public SpellDefinition fireballSpell;
	    public int maxFireballCharges = 5;
	    public int currentFireballCharges = 5;
	    public float fireballChargesCooldown = 3f;
	    public float currentFireballCooldown = 0;

        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnDash = delegate { };
        public event Action OnStun = delegate { };
        public event Action OnSpecialAttack = delegate { };
        public event Action OnChargedAttack = delegate { };
        public event Action OnShowDamage = delegate { };
        public event Action OnSpellAiming = delegate { };

        public void DmgDdisp(Vector3 direction)
        {
            this.EntityMove.SmoothMoveTransform(transform.position + direction * DmgDispl, 0.1f);
            this.EntityMove.RotateInstant(direction);
        }
        private void AtkDdisp()
        {
            this.EntityMove.SmoothMoveTransform(transform.position + transform.forward * DmgDispl, 0.1f);
        }
        public override void TakeDamage(int damage, DamageType type)
	    {
            OnShowDamage();
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
		    currentFireballCharges = maxFireballCharges;
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
		    /*if (InputManager.Instance.ChargedAttackDown && OnChargedAttack != null)
		    {
			    OnChargedAttack();
		    }*/
		    if (InputManager.Instance.AbilityAim && OnSpellAiming != null)
		    {
			    OnSpellAiming();
		    }
		    
		    if (currentFireballCharges < maxFireballCharges)
		    {
			    if (currentFireballCooldown <= 0)
			    {
				    currentFireballCharges++;

				    currentFireballCooldown = fireballChargesCooldown;
			    }
			    
			    currentFireballCooldown -= Time.deltaTime;
		    }
		    else
		    {
			    currentFireballCooldown = fireballChargesCooldown;
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
