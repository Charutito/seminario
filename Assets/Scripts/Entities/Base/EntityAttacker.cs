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

        private Entity entity;
        private EntityMove entityMove;
        #endregion

       

        #region Light Attack
        public void OnLighDashEnd()
        {
            entity.Animator.SetFloat("Velocity Z", 0);
            entity.Animator.applyRootMotion = true;
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
                        entityMove.RotateInstant(enemy.transform.position);

                        // Si el enemigo está a una distancia mayor triggereamos el salto
                        if (Vector3.Distance(transform.position, enemy.transform.position) > 2f)
                        {
                            entity.Animator.applyRootMotion = false; 
                            entity.Animator.SetFloat("Velocity Z", 2f);
                            if (!enemy.GetComponent<EntityAttacker>().canBeCountered)
                            {
                                FrameUtil.AfterDelay(0.1f, () =>
                                {
                                    StartCoroutine(MoveToPosition(transform, enemy.transform.position - transform.forward, 0.4f));
                                });
                            }
                            else if (enemy.GetComponent<EntityAttacker>().canBeCountered)
                            {
                                StartCoroutine(MoveToPosition(transform, enemy.transform.position - transform.forward, 0.1f));
                            }
                            // gameObject.MoveTo(enemy.transform.position - transform.forward, 0.75f, iTween.EaseType.easeOutQuart, "OnLighDashEnd");
                        }
                        else
                        {
                            entity.Animator.SetTrigger("LightAttack");
                        } 
                    }
                    else
                    {
                        entity.Animator.SetTrigger("LightAttack");

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
            entity.Animator.SetTrigger("LightAttack");
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
                damageable.TakeDamage((int)entity.Stats.Damage.Min);
            }
        }
        #endregion


        #region Heavy Attack
        public void HeavyAttack_Start()
        {
            entity.Animator.SetTrigger("HeavyAttack");
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
                    damageable.TakeDamage((int)entity.Stats.Damage.Max, h_damageType);
                }
            }
        }
        #endregion

        private void Start()
        {
            entity = GetComponent<Entity>();
            entityMove = GetComponent<EntityMove>();
        }
    }
}