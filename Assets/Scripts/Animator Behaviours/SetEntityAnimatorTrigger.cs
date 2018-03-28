using Entities;
using UnityEngine;

namespace Animator_Behaviours
{
    public class SetEntityAnimatorTrigger : StateMachineBehaviour
    {
        [SerializeField] private string trigger = "";
        [SerializeField] private bool triggerOnEnter;
        [SerializeField] private bool triggerOnExit;

        private Entity _entity;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_entity == null)
            {
                _entity = animator.GetComponentInParent<Entity>();
            }

            if (triggerOnEnter && _entity != null)
            {
                _entity.Animator.ResetTrigger(trigger);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (triggerOnExit && _entity != null)
            {
                _entity.Animator.ResetTrigger(trigger);
            }
        }
    }
}