﻿using FSM;
using Managers;
using System;
using System.Runtime.Remoting.Messaging;
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
	    public float currentDashCooldown;
	    
        [Header("GetHit")]
        public float getHitDuration = 0.7f;
        [Range(0,1)]
        public float AtkDispl= 0.5f;
        [Range(0, 1)]
        public float DmgDispl = 0.5f;

	    [Header("Spells")]
	    public AudioSource noSpiritSound;
        public Transform castPosition;
	    public SpellDefinition firstAbility;
	    public SpellDefinition secondAbilityAbility;

        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnDash = delegate { };
        public event Action OnStun = delegate { };
        public event Action OnSpecialAttack = delegate { };
        public event Action OnShowDamage = delegate { };
        public event Action OnGettingHitBack = delegate { };
        public event Action OnGetHit = delegate { };
        public event Action OnSpiritPunch = delegate { };

        public void DmgDdisp(Vector3 direction)
        {
            this.EntityMove.SmoothMoveTransform(transform.position + direction * DmgDispl, 0.1f);
            this.EntityMove.RotateInstant(direction);
        }
        public void Heal(float amount)
        {
            Stats.Health.Current += amount;
        }
        public void HealEnergy(float amount)
        {
            Stats.Spirit.Current += amount;
        }
        public void AtkDdisp()
        {
            EntityMove.SmoothMoveTransform(transform.position + transform.forward * DmgDispl, 0.1f);
        }

        public void SecondAbility()
        {
            SpellDefinition.Cast(secondAbilityAbility, transform);
        }

        public override void TakeDamage(Damage damage)
	    {
            OnShowDamage();

            if (damage.type == DamageType.ThirdAttack)
            {
                OnGettingHitBack();
            }
            else
            {
                OnGetHit();
            }

            if (damage.type == DamageType.SpecialAttack)
		    {
			    OnStun();

			    if (IsSpecialAttacking)
			    {
				    damage.amount = 0;
			    }
		    }
            base.TakeDamage(damage);
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

		    if (InputManager.Instance.FirstAbility)
		    {
			    SpellDefinition.Cast(firstAbility, transform);
		    }
		    
		    if (InputManager.Instance.SecondAbility)
		    {
                OnSpiritPunch();
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
