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

		private Vector3 _lastPosition;

		public void ShootAnimationEvent()
		{
			var newBomb = Instantiate(Bullet, BulletSpawnPos.position, _stateManager.Entity.transform.rotation);
			var newBombMove = newBomb.GetComponent<BombMove>();
			newBombMove.SetTargetPosition(_lastPosition);
		}

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<BomberStateManager>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_stateManager.Entity.Animator.SetTrigger(EntityAnimations.Attack);
				
				_stateManager.CurrentBullets--;
				_stateManager.Entity.EntityMove.RotateInstant(_stateManager.Entity.Target.transform.position);

				_lastPosition = _stateManager.Entity.Target.transform.position;
			};
		}
	}
}
