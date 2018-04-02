using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class EventFSM<T>
    {
	    protected string debugName = "FSM";
	    private readonly State<T> _any;
	    private State<T> _current;
	    private bool _feeding = false;

        public State<T> Current { get { return _current; } }

	    private bool _enabled;

        public EventFSM( State<T> any = null)
        {
            this._any = any ?? new State<T>("<any>");
            this._any.OnEnter += () => { throw new Exception("Can't make transition to fsm's <any> state"); };
        }

        public EventFSM(State<T> initial, State<T> any = null)
        {
            SetInitialState(initial);
			this._any = any ?? new State<T>("<any>");
			this._any.OnEnter += () => { throw new Exception("Can't make transition to fsm's <any> state"); };
		}

	    protected void SetInitialState(State<T> initial)
        {
            _current = initial;
            _current._Enter();
            _enabled = true;
        }

        public virtual void Update()
        {
            if(_enabled) _current._Update();
		}

	    protected bool Feed(T input)
        {
	        if (_feeding) throw new Exception("Error: Feeding from OnEnter or OnExit, will cause repeated or missing calls." + " Feed: " + input);
			
			State<T>.Transition transition;
			if(_current._TryGetTransition(input, out transition) || _any._TryGetTransition(input, out transition))
			{
				_feeding = true;		//Not multi-thread safe...

				_current._Exit();
				#if FP_VERBOSE
				    Debug.Log(debugName + " State: " + _current.name + "---" + input + "---> " + transition.targetState.name);
				#endif
				transition._Transition();
				_current = transition.targetState;
				_current._Enter();
				
				_feeding = false;

				return true;
			}
			return false;
		}
	}
}