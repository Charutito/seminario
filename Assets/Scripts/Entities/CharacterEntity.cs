using FSM;
using Managers;
using System;
using System.Runtime.Remoting.Messaging;
using BattleSystem;
using UnityEngine;
using Util;

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

	    public VfxManager vfxManager;

	    [Header("Spells")]
	    public GameObject CastPosition;
	    public SpellDefinition FirstAbility;
	    public SpellDefinition SecondAbility;
	    public SpellDefinition ThirdAbility;
	    public SpellDefinition FourthAbility;
	    
	    [HideInInspector] public float CurrentFirstAbilityCooldown;
	    [HideInInspector] public float CurrentSecondAbilityCooldown;
	    [HideInInspector] public float CurrentThirdAbilityCooldown;
	    [HideInInspector] public float CurrentFourthAbilityCooldown;
	    
	    [HideInInspector] public float CurrentTimeToCastFirstAbility;

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
        public event Action OnFireballCast = delegate { };
        public event Action OnBackflipCast = delegate { };

        public void DmgDdisp(Vector3 direction)
        {
	        if (IsInvulnerable) return;
            EntityMove.SmoothMoveTransform(transform.position + direction * DmgDispl, 0.1f);
            EntityMove.RotateInstant(direction);
        }
        
        public void AtkDdisp()
        {
            EntityMove.SmoothMoveTransform(transform.position + transform.forward * transform.GetMaxDistance(DmgDispl), 0.1f);
        }
	    
	    public override void Heal(int amount)
	    {
		    InputManager.Instance.Vibrate(0, 0.3f, 0.15f);
		    base.Heal(amount);
	    }
	    public override void HealEnergy(int amount)
	    {
		    InputManager.Instance.Vibrate(0, 0.3f, 0.15f);
		    base.HealEnergy(amount);
	    }
	    
	    public void FirstAbilityHit()
	    {
		    CurrentFirstAbilityCooldown = FirstAbility.Cooldown;
		    EntityMove.RotateInstant(EntityAttacker.lineArea.HitCollider.transform.position);

		    if (CurrentTimeToCastFirstAbility <= 0.5f)
		    {
			    Stats.CurrentSpirit -= FirstAbility.SpellVariation.SpiritCost;
			    SpellDefinition.Cast(FirstAbility.SpellVariation, CastPosition.transform.position, transform.rotation);
		    }
		    else
		    {
			    Stats.CurrentSpirit -= FirstAbility.SpiritCost;
			    SpellDefinition.Cast(FirstAbility, CastPosition.transform.position, transform.rotation);
		    }
	    }

        public void SecondAbilityHit()
        {
	        Stats.CurrentSpirit -= SecondAbility.SpiritCost;
	        CurrentSecondAbilityCooldown = SecondAbility.Cooldown;
	        EntityMove.RotateInstant(EntityAttacker.lineArea.HitCollider.transform.position);
            SpellDefinition.Cast(SecondAbility, transform);
        }

	    public void DancingBladesStart()
	    {
		    Stats.CurrentSpirit -= ThirdAbility.SpiritCost;
		    CurrentThirdAbilityCooldown = ThirdAbility.Cooldown;
		    SpellDefinition.Cast(ThirdAbility, transform, true);
	    }
	    
	    public void FourthAbilityHit()
	    {
		    Stats.CurrentSpirit -= FourthAbility.SpiritCost;
		    CurrentFourthAbilityCooldown = FourthAbility.Cooldown;
		    EntityMove.RotateInstant(EntityAttacker.lineArea.HitCollider.transform.position);
		    SpellDefinition.Cast(FourthAbility, transform, true);
	    }

        public override void TakeDamage(Damage damage)
        {
	        if (IsInvulnerable) return;
	        
            OnShowDamage();

            if (damage.Type == DamageType.ThirdAttack)
            {
                OnGettingHitBack();
            }
            else
            {
                OnGetHit();
            }

            if (damage.Type == DamageType.SpecialAttack)
		    {
			    OnStun();

			    if (IsSpecialAttacking)
			    {
				    damage.Amount = 0;
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
		    
		    if (InputManager.Instance.FirstAbility && Stats.CurrentSpirit >= FirstAbility.SpiritCost && CurrentFirstAbilityCooldown <= 0 && !IsDead)
		    {
			    OnFireballCast();
		    }

		    if (InputManager.Instance.SecondAbility && Stats.CurrentSpirit >= SecondAbility.SpiritCost && CurrentSecondAbilityCooldown <= 0 && !IsDead)
		    {
                OnSpiritPunch();
		    }
		    if (InputManager.Instance.ThirdAbility && Stats.CurrentSpirit >= ThirdAbility.SpiritCost && CurrentThirdAbilityCooldown <= 0 && !IsDead)
		    {
			    OnDancingBlades();
		    }
		    if (InputManager.Instance.FourthAbility && Stats.CurrentSpirit >= FourthAbility.SpiritCost && CurrentFourthAbilityCooldown <= 0  && !IsDead)
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
