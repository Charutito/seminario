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
        private BasicEnemyStateManager _stateManager;

        protected override void Setup()
        {
            _stateManager = GetComponent<BasicEnemyStateManager>();
        }

        protected override void DefineState()
        {
            OnEnter += () =>
            {
                _stateManager.Entity.Animator.SetBool(EntityAnimations.Death, true);
                _stateManager.Entity.Animator.SetTrigger(EntityAnimations.TriggerDeath);
                _stateManager.Entity.Animator.SetInteger(EntityAnimations.RandomDeath, Random.Range(0, 3));
                _stateManager.Entity.Agent.enabled = false;
                _stateManager.Entity.Collider.enabled = false;
                _stateManager.FSM.enabled = false;

                FrameUtil.AfterDelay(1f, () => GameManager.Instance.RunCoroutine(DisolveCorroutine()));
                Destroy(_stateManager.Entity.gameObject, 3f);
            };
        }

        private IEnumerator DisolveCorroutine()
        {
            var mesh = _stateManager.Entity.GetComponentInChildren<SkinnedMeshRenderer>();
            
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
