using BattleSystem;
using FSM;
using Steering;
using UnityEngine;

namespace Entities
{
	public class BlockEnemy : BasicEnemy
	{
		public int shieldHealth = 100;
		public int currentShieldHealth;

		private LineOfSight lineOfSight;
		
		public override void TakeDamage(int damage, DamageType type)
		{
			if (IsBlocking && lineOfSight.TargetInSight)
			{
				type = DamageType.Block;
				currentShieldHealth -= damage;
				damage = 0;
			}
			else
			{
				if (!lineOfSight.TargetInSight)
				{
					damage *= 2;
					Animator.SetTrigger("BackHit");
					type = DamageType.Back;
				}
				
				HitFeedback();
			}

			base.TakeDamage(damage, type);
		}

		protected override void SetFsm()
		{
			EntityFsm = new BlockEnemyFSM(this);
		}

		protected override void Start()
		{
			lineOfSight = GetComponent<LineOfSight>();
			
			currentShieldHealth = shieldHealth;
			base.Start();
		}
	}
}