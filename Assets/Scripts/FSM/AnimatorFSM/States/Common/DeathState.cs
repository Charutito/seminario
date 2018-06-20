using System.Collections;
using Entities.Base;
using Managers;
using UnityEngine;
using Util;

namespace AnimatorFSM.States
{
    [AddComponentMenu("State Machine/Death State")]
    public class DeathState : BaseState
    {
        protected override void Setup() {}

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                StateManager.Entity.Animator.SetBool(EntityAnimations.Death, true);
                StateManager.Entity.Animator.SetTrigger(EntityAnimations.TriggerDeath);
                StateManager.Entity.Animator.SetInteger(EntityAnimations.RandomDeath, Random.Range(0, 3));
                StateManager.Entity.Agent.enabled = false;
                StateManager.Entity.Collider.enabled = false;
                StateManager.FSM.enabled = false;

                FrameUtil.AfterDelay(1f, () => CoroutineManager.Instance.RunCoroutine(DisolveCorroutine()));
                Destroy(StateManager.Entity.gameObject, 3f);
            };
        }

        private IEnumerator DisolveCorroutine()
        {
            var mesh = StateManager.Entity.GetComponentInChildren<SkinnedMeshRenderer>();

            if (mesh != null)
            {
                float disolve = 0;
            
                while (disolve < 1)
                {
                    disolve += 0.01f;
                    mesh.material.SetFloat("_Disolve", disolve);
                    yield return null;
                }
            }
        }
    }
}
