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
		
		public override void TakeDamage(Damage damage)
		{
			if (IsBlocking && lineOfSight.TargetInSight)
			{
				damage.type = DamageType.Block;
				currentShieldHealth -= damage.amount;
				damage.amount = 0;
			}
			else
			{
				if (!lineOfSight.TargetInSight)
				{
					damage.amount *= 2;
					Animator.SetTrigger("BackHit");
					damage.type = DamageType.Back;
				}
				
				HitFeedback();
			}

			base.TakeDamage(damage);
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