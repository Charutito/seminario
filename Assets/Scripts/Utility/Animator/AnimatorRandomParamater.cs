using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class AnimatorRandomParamater : StateMachineBehaviour
    {
        [MinMaxRange(0, 10)]
        public RangedFloat Range;
        
        public string ParameterName = string.Empty;
        public bool IsFloat = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (ParameterName == string.Empty) return;
            
            if (IsFloat)
            {
                animator.SetFloat(ParameterName, Range.GetRandom);
            }
            else
            {
                animator.SetInteger(ParameterName, Range.GetRandomInt);
            }
        }
    }
}
