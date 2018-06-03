using System;
using UnityEngine;
using Util;

namespace AnimatorFSM.States
{
	public abstract class BaseState : MonoBehaviour
	{
		public event Action OnEnter = delegate {};
		public event Action OnUpdate = delegate {};
		public event Action OnExit = delegate {};
		
		protected AbstractStateManager StateManager;
		
		private bool _isFirstRun = true;
		
		protected abstract void	Setup();
		protected abstract void DefineState();

		private void Awake()
		{
			StateManager = GetComponent<AbstractStateManager>();
			
			Setup();
		}

		private void Start()
		{
			DefineState();
			_isFirstRun = false;
			FrameUtil.OnNextFrame(OnEnable);
		}
		
		private void OnEnable()
		{
			if(OnEnter != null && !_isFirstRun) OnEnter();
		}

		private void Update()
		{
			if(OnUpdate != null && !_isFirstRun) OnUpdate();
		}

		private void OnDisable()
		{
			if(OnExit != null) OnExit();
		}
	}
}
