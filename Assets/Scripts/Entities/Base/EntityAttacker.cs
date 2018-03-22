﻿using BattleSystem;
using EZCameraShake;
using UnityEngine;
using GameUtils;
using Util;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Entities
{
    [RequireComponent(typeof(EntityMove))]
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars
        [SerializeField] private ColliderObserver attackArea;
        [SerializeField] private LineOfAim lineArea;
        [SerializeField] private float heavyAttackRadious = 5f;
        public bool canBeCountered = false;

        [Header("Heavy Attack")]
        [SerializeField] private float h_magn = 1;
        [SerializeField] private float h_rough = 1;
        [SerializeField] private float h_fadeIn = 0.1f;
        [SerializeField] private float h_fadeOut = 2f;
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private DamageType h_damageType = DamageType.Unknown;

        private Entity _entity;
        private EntityMove _entityMove;
        #endregion

       

        #region Light Attack
        public void OnLighDashEnd()
        {
            _entity.Animator.SetFloat("Velocity Z", 0);
            _entity.Animator.applyRootMotion = true;
        }

        public void LightAttack_Start()
        {
            if (lineArea != null)
            {
                lineArea.GetEnemiesInSight((enemies) =>
                {
                    canBeCountered = true;
                    var enemy = enemies
                                    .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
                                    .FirstOrDefault();
                    if (enemy != null)
                    {
                        _entityMove.RotateInstant(enemy.transform.position);

                        // Si el enemigo está a una distancia mayor triggereamos el salto
                        if (Vector3.Distance(transform.position, enemy.transform.position) > 2f)
                        {
                            _entity.Animator.applyRootMotion = false; 
                            _entity.Animator.SetFloat("Velocity Z", 1.5f);
                            if (!enemy.GetComponent<EntityAttacker>().canBeCountered)
                            {
                                FrameUtil.AfterDelay(0.1f, () =>
                                {
                                    StartCoroutine(MoveToPosition(transform, enemy.transform.position - transform.forward, 0.75f));
                                    _entity.Animator.SetTrigger("RunAttack");

                                });
                            }
                            else if (enemy.GetComponent<EntityAttacker>().canBeCountered)
                            {
                                StartCoroutine(MoveToPosition(transform, enemy.transform.position - transform.forward, 0.1f));
                                _entity.Animator.SetTrigger("LightAttack");
                            }
                            // gameObject.MoveTo(enemy.transform.position - transform.forward, 0.75f, iTween.EaseType.easeOutQuart, "OnLighDashEnd");
                        }
                        else
                        {
                            _entity.Animator.SetTrigger("LightAttack");
                        } 
                    }
                    else
                    {
                        _entity.Animator.SetTrigger("LightAttack");

                    }
                });
            }
        }
         public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
        {
            var currentPos = transform.position;
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }
            OnLighDashEnd();
        }

        public void LightAttack_Hit()
        {
            attackArea.gameObject.SetActive(true);
            canBeCountered = false;
            attackArea.TriggerEnter += LightAttack_Damage;
            FrameUtil.AtEndOfFrame(() => 
            {
                attackArea.TriggerEnter -= LightAttack_Damage;
                attackArea.gameObject.SetActive(false);
            });
        }

        private void LightAttack_Damage(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage((int)_entity.Stats.Damage.Min);
            }
        }
        #endregion


        #region Heavy Attack
        public void HeavyAttack_Start()
        {
            //LookToMouse();
            canBeCountered = true;

        }

        public void HeavyAttack_Hit()
        {
            CameraShaker.Instance.ShakeOnce(h_magn, h_rough, h_fadeIn, h_fadeOut);
            canBeCountered = false;
            HeavyAttack_Damage();
        }

        private void HeavyAttack_Damage()
        {

            var colliders = Physics.OverlapSphere(attackArea.transform.position, heavyAttackRadious, hitLayers);
            foreach (var collider in colliders)
            {
                var damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage((int)_entity.Stats.Damage.Max, h_damageType);
                }
            }
        }
        #endregion

        private void Start()
        {
            _entity = GetComponent<Entity>();
            _entityMove = GetComponent<EntityMove>();
        }
    }
}
