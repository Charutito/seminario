using BattleSystem;
using FSM;

namespace Entities
{
	public class BlockEnemy : BasicEnemy
	{
		public override void TakeDamage(int damage, DamageType type)
		{
			if (type == DamageType.SpecialAttack ||
			    (IsAttacking && (type == DamageType.Attack)))
			{
				type = DamageType.Block;
				damage = 0;
			}

			base.TakeDamage(damage, type);
		}

		protected override void SetFsm()
		{
			EntityFsm = new BasicEnemyFSM(this);
		}
	}
}