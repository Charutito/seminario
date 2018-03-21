using UnityEngine;

public class ClearTriggerOnExit : StateMachineBehaviour
{
    [SerializeField] private string triggerToReset = "";
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerToReset);
	}
}
