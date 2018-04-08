using BattleSystem;
using EZCameraShake;
using UnityEngine;
using GameUtils;
using Util;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Entities
{
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars
        public ColliderObserver attackArea;
        public LineOfAim lineArea;
        public float heavyAttackRadious = 5f;

        [Header("Heavy Attack")]
        [SerializeField] private float h_magn = 1;
        [SerializeField] private float h_rough = 1;
        [SerializeField] private float h_fadeIn = 0.1f;
        [SerializeField] private float h_fadeOut = 2f;
        [SerializeField] private LayerMask hitLayers;

        private Entity _entity;
        #endregion
        
        #region Light Attack
        public void OnLighDashEnd()
        {
            _entity.Animator.SetTrigger("Attack");
            _entity.Animator.SetFloat("Velocity Z", 0);
            _entity.Animator.applyRootMotion = true;
        }

        public void LightAttack_Start()
        {
            if (lineArea != null)
            {
                lineArea.GetEnemiesInSight((enemies) =>
                {
                    var enemy = enemies
                                    .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
                                    .FirstOrDefault();
                    
                    if (enemy != null)
                    {
							_entity.EntityMove.RotateInstant(enemy.transform.position);

                        // Si el enemigo está a una distancia mayor triggereamos el salto
                        if (Vector3.Distance(transform.position, enemy.transform.position) > 2f)
                        {
                            _entity.Animator.applyRootMotion = false; 
                            _entity.Animator.SetFloat("Velocity Z", 2f);
                            _entity.EntityMove.SmoothMoveTransform(enemy.transform.position - transform.forward, 0.1f, OnLighDashEnd);
                        }
                        else
                        {
                            _entity.Animator.SetTrigger("Attack");
                        } 
                    }
                    else
                    {
                        _entity.Animator.SetTrigger("Attack");
                    }
                });
            }
        }

        public void LightAttack_Hit()
        {
            attackArea.TriggerEnter += LightAttack_Damage;
            attackArea.gameObject.SetActive(true);

            FrameUtil.AfterFrames(3, () => 
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
                damageable.TakeDamage(_entity.AttackDamage, DamageType.Attack);
            }
        }
        #endregion


        #region Heavy Attack
        public void HeavyAttack_Start()
        {
            _entity.Animator.SetTrigger("SpecialAttack");
        }

        public void HeavyAttack_Hit()
        {
            CameraShaker.Instance.ShakeOnce(h_magn, h_rough, h_fadeIn, h_fadeOut);
			attackArea.TriggerEnter += HeavyAttack_Damage;
			attackArea.gameObject.SetActive(true);

			FrameUtil.AfterFrames(3, () => 
			{
				attackArea.TriggerEnter -= HeavyAttack_Damage;
				attackArea.gameObject.SetActive(false);
			});
        }

		private void HeavyAttack_Damage(Collider other)
        {
            //var colliders = Physics.OverlapSphere(attackArea.transform.position, heavyAttackRadious, hitLayers);
            var damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_entity.HeavyAttackDamage, DamageType.SpecialAttack);
            }
        }
        #endregion

        #region Charged Attack
        public void ChargedAttack_Hit()
        {
            var colliders = Physics.OverlapSphere(attackArea.transform.position, heavyAttackRadious, hitLayers);

            foreach (var other in colliders)
            {
                var damageable = other.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(_entity.HeavyAttackDamage, DamageType.ChargedAttack);
                }
            }
        }
        #endregion

        private void Start()
        {
            _entity = GetComponent<Entity>();
        }
    }
}
