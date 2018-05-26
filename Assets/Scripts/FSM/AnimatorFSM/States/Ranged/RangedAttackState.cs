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
				Instantiate(Bullet, BulletSpawnPos.position, _stateManager.Entity.transform.rotation);
				_stateManager.Entity.EntitySounds.PlayEffect("Shoot", transform.position);
			};
		}
	}
}
