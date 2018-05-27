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

		[Header("SFX")]
		public SimpleAudioEvent ShieldActivate;
		public SimpleAudioEvent ShieldDeactivate;
		public SimpleAudioEvent ReloadEffect;
		
		private BomberStateManager _stateManager;
		private GameObject _shieldMesh;
		private NavMeshObstacle _shieldObstacle;

		protected override void Setup()
		{
			_stateManager = GetComponentInParent<BomberStateManager>();
			_shieldMesh = Shield.transform.GetChild(0).gameObject;
			_shieldObstacle = Shield.GetComponent<NavMeshObstacle>();
		}

		protected override void DefineState()
		{
			OnEnter += () =>
			{
				ShieldActivate.PlayAtPoint(transform.position);
				
				FrameUtil.AfterDelay(1f, () => ReloadEffect.PlayAtPoint(transform.position));
				
				_stateManager.Entity.Agent.enabled = false;
				_shieldObstacle.enabled = true;
				
				_stateManager.Entity.IsInvulnerable = true;
				
				iTween.ScaleTo(_shieldMesh, iTween.Hash("scale", new Vector3(ShieldRange.maxValue, ShieldRange.maxValue, ShieldRange.maxValue), "time", RangeExpandTime, "easeType", iTween.EaseType.easeInSine));
			};
			
			OnExit += () =>
			{
				ShieldDeactivate.PlayAtPoint(transform.position);
				_stateManager.CurrentBullets = _stateManager.StartingBullets;
				
				_shieldObstacle.enabled = false;
				_stateManager.Entity.IsInvulnerable = false;
				
				FrameUtil.AfterDelay(0.5f, () => _stateManager.Entity.Agent.enabled = true);
				
				iTween.ScaleTo(_shieldMesh, iTween.Hash("scale", new Vector3(ShieldRange.minValue, ShieldRange.minValue, ShieldRange.minValue), "time", RangeExpandTime, "easeType", iTween.EaseType.easeInSine));
			};
		}
	}
}
