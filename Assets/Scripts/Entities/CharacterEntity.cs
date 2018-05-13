using FSM;
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
	    public SpellDefinition FirstAbility;
	    public SpellDefinition SecondAbility;
	    public SpellDefinition ThirdAbility;
	    public SpellDefinition FourthAbility;
	    
	    [Header("Cooldowns (Debug)")]
	    public float CurrentFirstAbilityCooldown;
	    public float CurrentSecondAbilityCooldown;
	    public float CurrentThirdAbilityCooldown;
	    public float CurrentFourthAbilityCooldown;

        public event Action OnMove = delegate { };
        public event Action OnAttack = delegate { };
        public event Action OnDash = delegate { };
        public event Action OnStun = delegate { };
        public event Action OnSpecialAttack = delegate { };
        public event Action OnShowDamage = delegate { };
        public event Action OnGettingHitBack = delegate { };
        public event Action OnGetHit = delegate { };
        public event Action OnSpiritPunch = delegate { };
        public event Action OnDancingBlades = delegate { };
        public event Action OnGravitonCast = delegate { };
        public event Action OnBackflipCast = delegate { };

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
	    
	    public void FirstAbilityHit()
	    {
		    CurrentFirstAbilityCooldown = FirstAbility.Cooldown;
		    Stats.Spirit.Current -= FirstAbility.SpiritCost;
		    SpellDefinition.Cast(FirstAbility, transform);
	    }

        public void SecondAbilityHit()
        {
	        Stats.Spirit.Current -= SecondAbility.SpiritCost;
	        CurrentSecondAbilityCooldown = SecondAbility.Cooldown;
            SpellDefinition.Cast(SecondAbility, transform);
        }

	    public void DancingBladesStart()
	    {
		    Stats.Spirit.Current -= ThirdAbility.SpiritCost;
		    CurrentThirdAbilityCooldown = ThirdAbility.Cooldown;
		    SpellDefinition.Cast(ThirdAbility, transform, true);
	    }
	    
	    public void FourthAbilityHit()
	    {
		    Stats.Spirit.Current -= FourthAbility.SpiritCost;
		    CurrentFourthAbilityCooldown = FourthAbility.Cooldown;
		    SpellDefinition.Cast(FourthAbility, transform, true);
	    }

        public override void TakeDamage(Damage damage)
        {
	        if (IsInvulnerable) return;
	        
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
		    if (InputManager.Instance.AxisMoving)
		    {
			    OnMove();
		    }
		    if (InputManager.Instance.Attack)
		    {
			    OnAttack();
		    }
		    if (InputManager.Instance.Dash)
		    {
			    OnDash();
		    }
		    if (InputManager.Instance.SpecialAttack)
		    {
			    OnSpecialAttack();
		    }
		    
		    if (InputManager.Instance.FirstAbility && Stats.Spirit.Current >= FirstAbility.SpiritCost && CurrentFirstAbilityCooldown <= 0 && !IsDead)
		    {
			    OnGravitonCast();
		    }
		    if (InputManager.Instance.SecondAbility && Stats.Spirit.Current >= SecondAbility.SpiritCost && CurrentSecondAbilityCooldown <= 0 && !IsDead)
		    {
                OnSpiritPunch();
		    }
		    if (InputManager.Instance.ThirdAbility && Stats.Spirit.Current >= ThirdAbility.SpiritCost && CurrentThirdAbilityCooldown <= 0 && !IsDead)
		    {
			    OnDancingBlades();
		    }
		    if (InputManager.Instance.FourthAbility && Stats.Spirit.Current >= FourthAbility.SpiritCost && CurrentFourthAbilityCooldown <= 0  && !IsDead)
		    {
			    OnBackflipCast();
		    }

		    UpdateCooldowns();

		    base.Update();
	    }

	    private void UpdateCooldowns()
	    {
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
		    
		    if (CurrentFirstAbilityCooldown > 0)
		    {
			    CurrentFirstAbilityCooldown -= Time.deltaTime;
		    }
		    if (CurrentSecondAbilityCooldown > 0)
		    {
			    CurrentSecondAbilityCooldown -= Time.deltaTime;
		    }
		    if (CurrentThirdAbilityCooldown > 0)
		    {
			    CurrentThirdAbilityCooldown -= Time.deltaTime;
		    }
		    if (CurrentFourthAbilityCooldown > 0)
		    {
			    CurrentFourthAbilityCooldown -= Time.deltaTime;
		    }
	    }
    }
}
