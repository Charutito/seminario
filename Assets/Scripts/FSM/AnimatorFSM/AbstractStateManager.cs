using Entities;
using UnityEngine;

namespace AnimatorFSM
{
	public class AbstractStateManager : MonoBehaviour
	{
		public bool StateLocked { get; set; }
		
		public BasicEnemy Entity { get; private set; }
		public Animator FSM { get; private set; }
		
		protected void SetState(string state, bool force = false)
		{
			if(!StateLocked || force) FSM.SetTrigger(state);
		}

		protected void Awake()
		{
			Entity = GetComponentInParent<BasicEnemy>();
			FSM = GetComponent<Animator>();
		}
	}
}
