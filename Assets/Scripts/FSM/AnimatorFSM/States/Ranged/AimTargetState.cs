using AnimatorFSM.States;
using Entities.Base;

namespace AnimatorFSM.States
{
	public class AimTargetState : BaseState
	{
		private AbstractStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<AbstractStateManager>();
		}
	
		protected override void DefineState()
		{
			OnEnter += () => _stateManager.Entity.Animator.SetBool(EntityAnimations.Aim, true);

			OnUpdate += () =>
			{
				_stateManager.Entity.EntityMove.RotateTowards(_stateManager.Entity.Target.transform.position);
			};
			
			OnExit += () => _stateManager.Entity.Animator.SetBool(EntityAnimations.Aim, false);
		}
	}
}
