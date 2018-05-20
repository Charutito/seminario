using System.Collections;
using Entities.Base;
using UnityEngine;

namespace AnimatorFSM.States
{
	public class BomberAttackState : BaseState
	{
		public GameObject Bullet;
		public Transform BulletSpawnPos;
		
		private BomberStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<BomberStateManager>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_stateManager.CurrentBullets--;
				_stateManager.Entity.EntityMove.RotateInstant(_stateManager.Entity.Target.transform.position);
				//_stateManager.Entity.Animator.SetTrigger(EntityAnimations.Attack);
				
				var newBomb = Instantiate(Bullet, BulletSpawnPos.position, _stateManager.Entity.transform.rotation);
				var newBombMove = newBomb.GetComponent<BombMove>();
				newBombMove.SetTargetPosition(_stateManager.Entity.Target.transform.position);
			};
		}
	}
}
