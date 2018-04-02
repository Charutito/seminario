using FSM;
using Managers;
using UnityEngine;
using System;


namespace Entities
{
   
        

    public class BasicRangedEnemy : BasicEnemy
    {

        [Range(1f, 5f)]
        public float RangeToAim;
        public float MaxAimTime;
        

        void Start () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		
	}
}
}

