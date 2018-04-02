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
       [Range(0.5f, 5f)]
        public float MaxAimTime;
        

	
	// Update is called once per frame
	protected override void Update () {
		
	}
}
}

