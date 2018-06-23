using BattleSystem;
using Entities.Base;
using UnityEngine;
using Util;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Mine Death State")]
	public class MineDeathState : DeathState
	{
		[Header("Explosion")]
		public LayerMask HitLayers;
		public float ExplosionRange = 4f;
		public float ExplosionDisplacement = 2f;
		
		protected override void DefineState()
		{
			OnEnter += () =>
			{
				StateManager.Entity.SelfDestroy();
				StateManager.Entity.Agent.enabled = false;
				StateManager.FSM.enabled = false;
				
				var colliders = Physics.OverlapSphere(StateManager.Entity.transform.position, ExplosionRange, HitLayers);

				foreach (var other in colliders)
				{
					var damageable = other.GetComponent<IDamageable>();

					if (damageable != null)
					{
						var damage = new Damage
						{
							Amount = StateManager.Entity.Stats.LightAttackDamage,
							Displacement = ExplosionDisplacement,
							OriginPosition = StateManager.Entity.transform.position,
							OriginRotation = StateManager.Entity.transform.rotation,
							Type = DamageType.ThirdAttack
						};
							
						damageable.TakeDamage(damage);
					}
				}
				
				var mesh = StateManager.Entity.GetComponentInChildren<MeshRenderer>();
				mesh.enabled = false;
				
				Destroy(StateManager.Entity.gameObject, 2f);
			};
		}
	}
}
