using System.Collections;
using Entities.Base;
using UnityEngine;
using UnityEngine.AI;
using Util;

namespace AnimatorFSM.States
{
	public class ShieldState : BaseState
	{
		[MinMaxRange(0, 10)]
		public RangedFloat ShieldRange;
		public float RangeExpandTime;
		
		public GameObject Shield;
		
		private BomberStateManager _stateManager;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<BomberStateManager>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				_stateManager.Entity.Agent.enabled = false;
				Shield.GetComponent<NavMeshObstacle>().enabled = true;
				
				_stateManager.Entity.IsInvulnerable = true;
				_stateManager.CurrentBullets = _stateManager.StartingBullets;
			};
			
			OnExit += () =>
			{
				_stateManager.Entity.IsInvulnerable = false;
				Shield.GetComponent<NavMeshObstacle>().enabled = false;
				FrameUtil.OnNextFrame(() => _stateManager.Entity.Agent.enabled = true);
			};
		}
	}
}
