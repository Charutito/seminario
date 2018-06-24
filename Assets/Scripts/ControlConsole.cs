using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControlConsole : MonoBehaviour {
    public event Action OnConsoleDestroy = ()=>{ };
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void OnDestroy()
    {
        OnConsoleDestroy();
    }
}
