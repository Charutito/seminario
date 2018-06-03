using Entities.Base;
using UnityEngine;
using Util;

namespace AnimatorFSM.States
{
	[AddComponentMenu("State Machine/Mine Death State")]
	public class MineDeathState : DeathState
	{
		protected override void DefineState()
		{
			OnEnter += () =>
			{
				StateManager.Entity.Agent.enabled = false;
				StateManager.FSM.enabled = false;
				
				var mesh = StateManager.Entity.GetComponentInChildren<MeshRenderer>();
				mesh.enabled = false;
				
				Destroy(StateManager.Entity.gameObject, 2f);
			};
		}
	}
}
