﻿using BattleSystem;
using EZCameraShake;
using UnityEngine;
using GameUtils;
using Util;
using System.Collections.Generic;
using BattleSystem.Spells;
using System.Collections;
using System.Linq;
using Managers;
using Utility;

namespace Entities
{
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars
        public ColliderObserver attackArea;
        public LineOfAim lineArea;
        public bool IsCharacter;

        [Header("Heavy Attack")]
        [SerializeField] private float heavyaoe = 2f;
        [SerializeField] private LayerMask hitLayers;
        
        [Header("Displacement")]
        [SerializeField] private float basicDisplacement = 0.7f;
        [SerializeField] private float heavyDisplacement = 1f;
        [SerializeField] private float thirdDisplacement = 2f;

        private Entity _entity;
        private CharacterEntity _character;
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
            /*if (lineArea != null)
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
            }*/

            if (IsCharacter)
            {
                _entity.Animator.SetTrigger("Attack");
                _entity.EntityMove.RotateInstant(_entity.transform.position + new Vector3(InputManager.Instance.AxisHorizontal, 0, InputManager.Instance.AxisVertical));
            }
        }

        public void LightAttack_Hit()
        {
            if (IsCharacter)
            {
                _character.AtkDdisp();
            }
            
            AttackAreaLogic(new Damage
            {
                Amount = _entity.Stats.LightAttackDamage,
                Type = DamageType.Attack,
                Displacement = basicDisplacement
            });
        }
        #endregion

        
        #region Third Attack
        public void ThirdAttack_Hit()
        {
            if (IsCharacter)
            {
                _character.AtkDdisp();
            }
            
            AttackAreaLogic(new Damage
            {
                Amount = _entity.Stats.SpecialAttackDamage,
                Type = DamageType.ThirdAttack,
                Displacement = thirdDisplacement
            }, true);
        }
        #endregion

        #region Fly Up Attack
        public void FlyUpAttack_Hit()
        {
            if (IsCharacter)
            {
                _character.AtkDdisp();
            }

            AttackAreaLogic(new Damage
            {
                Amount = _entity.Stats.SpecialAttackDamage,
                Type = DamageType.FlyUp,
                Displacement = basicDisplacement
            });
        }
        #endregion


        #region Heavy Attack
        public void HeavyAttack_Start()
        {
            _entity.Animator.SetTrigger("SpecialAttack");
        }

        public void HeavyAttack_Hit()
        {
            InputManager.Instance.Vibrate(0.7f, 0.3f, 0.2f);
            CinemachineShake.Instance.ShakeCamera(0.15f, 1.5f, 0.5f);
            
            AttackAreaLogic(new Damage
            {
                Amount = _entity.Stats.SpecialAttackDamage,
                Type = DamageType.SpecialAttack,
                Displacement = heavyDisplacement
            });
        }
        #endregion
        
        
        private void AttackAreaLogic(Damage damage, bool aoe = false)
        {
            var targets = new List<Collider>();

            if (aoe)
            {
                targets = Physics.OverlapSphere(attackArea.transform.position, heavyaoe, hitLayers).ToList();
            }
            else
            {
                var tarjetas = Physics.BoxCastAll(attackArea.transform.position, attackArea.transform.localScale / 2, transform.forward, attackArea.transform.rotation, 1, hitLayers);

                targets.AddRange(tarjetas.Select(target => target.collider));
            }

            damage.OriginPosition = transform.position;
            damage.OriginRotation = transform.rotation;
            
            foreach (var target in targets)
            {
                var damageable = target.GetComponent<IDamageable>();
                var character = target.GetComponent<CharacterEntity>();
                var bullet = target.GetComponent<BulletMove>();
            
                if (character != null)
                {
                    character.DmgDdisp(transform.forward);
                }

                if (bullet != null)
                {
                    bullet.TakeDamage();
                }

                if (damageable != null)
                {
                    if (IsCharacter)
                    {
                        var targetEntity = target.GetComponent<Entity>();
                        
                        if (targetEntity != null && !targetEntity.IsInvulnerable)
                        {
                            if(damage.Type != DamageType.SpecialAttack) InputManager.Instance.Vibrate(0.4f, 0.2f, 0.15f);
                        }
                        
                        var entity = target.GetComponent<Entity>();
                        
                        if (entity != null)
                        {
                            _entity.Stats.CurrentSpirit += (damage.Type == DamageType.SpecialAttack) ? _entity.Stats.HeavyRecovery : _entity.Stats.LightRecovery;
                        }
                    }
                    damageable.TakeDamage(damage);
                }
            }
        }

        private void Start()
        {
            _entity = GetComponent<Entity>();
            _character = GameManager.Instance.Character;
        }
    }
}
