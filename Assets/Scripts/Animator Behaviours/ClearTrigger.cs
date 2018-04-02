using UnityEngine;

namespace Animator_Behaviours
{
    public class ClearTrigger : StateMachineBehaviour
    {
        [SerializeField] private string triggerToReset = "";
        [SerializeField] private bool clearOnEnter;
        [SerializeField] private bool clearOnExit;


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (clearOnEnter)
            {
                animator.ResetTrigger(triggerToReset);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (clearOnExit)
            {
                animator.ResetTrigger(triggerToReset);
            }
        }
    }
}