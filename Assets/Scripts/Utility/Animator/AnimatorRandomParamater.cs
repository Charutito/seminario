using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class AnimatorRandomParamater : StateMachineBehaviour
    {
        [MinMaxRange(0, 10)]
        public RangedFloat Range;
        
        public string TriggerName = string.Empty;
        public bool IsFloat = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (TriggerName == string.Empty) return;
            
            if (IsFloat)
            {
                animator.SetFloat(TriggerName, Range.GetRandom);
            }
            else
            {
                animator.SetInteger(TriggerName, Range.GetRandomInt);
            }
        }
    }
}
