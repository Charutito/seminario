using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	public class RangedAttackState : BaseState
	{
		public GameObject Bullet;
		public Transform BulletSpawnPos;
		
		private AbstractStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<AbstractStateManager>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_stateManager.Entity.EntityMove.RotateInstant(_stateManager.Entity.Target.transform.position);
				_stateManager.Entity.Animator.SetTrigger(EntityAnimations.Attack);
				var bulletObject = Instantiate(Bullet, BulletSpawnPos.position, _stateManager.Entity.transform.rotation);

				var bulletDamage = bulletObject.GetComponent<BulletCollisionDamage>();
				bulletDamage.Damage = _stateManager.Entity.Stats.LightAttackDamage;
				
				_stateManager.Entity.EntitySounds.PlayEffect("Shoot", transform.position);
			};
		}
	}
}
