using BattleSystem;
using EZCameraShake;
using UnityEngine;
using GameUtils;
using Util;
using System.Collections.Generic;
using BattleSystem.Spells;
using System.Collections;
using System.Linq;
using Managers;

namespace Entities
{
    public class EntityAttacker : MonoBehaviour
    {
        #region Local Vars
        public ColliderObserver attackArea;
        public LineOfAim lineArea;
        public bool IsCharacter;

        [Header("Heavy Attack")]
        [SerializeField] private float h_magn = 1;
        [SerializeField] private float h_rough = 1;
        [SerializeField] private float h_fadeIn = 0.1f;
        [SerializeField] private float h_fadeOut = 2f;
        [SerializeField] private float heavyaoe = 2f;
        [SerializeField] private LayerMask hitLayers;

        [Header("Spirit Recovery")]
        [SerializeField] private int basicAttackSpirit = 5;
        [SerializeField] private int heavyAttackSpirit = 10;
        
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
                amount = _entity.AttackDamage,
                type = DamageType.Attack,
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
                amount = _entity.HeavyAttackDamage,
                type = DamageType.ThirdAttack,
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
                amount = _entity.HeavyAttackDamage,
                type = DamageType.FlyUp,
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
            // Temporary Broken
            //CameraShaker.Instance.ShakeOnce(h_magn, h_rough, h_fadeIn, h_fadeOut);
            InputManager.Instance.Vibrate(0.7f, 0.3f, 0.2f);
            
            AttackAreaLogic(new Damage
            {
                amount = _entity.HeavyAttackDamage,
                type = DamageType.SpecialAttack,
                Displacement = heavyDisplacement
            });
        }
        #endregion
        
        
        private void AttackAreaLogic(Damage damage, bool aoe = false)
        {
            List<Collider> targets = new List<Collider>();

            if (aoe)
            {
                targets = Physics.OverlapSphere(attackArea.transform.position, heavyaoe, hitLayers).ToList();
            }
            else
            {
                var tarjetas = Physics.BoxCastAll(attackArea.transform.position, attackArea.transform.localScale / 2, transform.forward, attackArea.transform.rotation, 1, hitLayers);

                foreach (var target in tarjetas)
                {
                    targets.Add(target.collider);
                }
            }
            
            damage.origin = transform;
            damage.originator = _entity;
            
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
                    bullet.ChangeDir();
                }

                if (damageable != null)
                {
                    if (IsCharacter)
                    {
                        var targetEntity = target.GetComponent<Entity>();
                        
                        if (targetEntity != null && !targetEntity.IsInvulnerable)
                        {
                            GameManager.Instance.Combo++;
                        }
                    }
                    _entity.Stats.Spirit.Current += (damage.type == DamageType.SpecialAttack) ? heavyAttackSpirit : basicAttackSpirit;
                    damageable.TakeDamage(damage);
                }
            }
        }

        private void Start()
        {
            _entity = GetComponent<Entity>();
            _character = GameManager.Instance.Character;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawRay(attackArea.transform.position, attackArea.transform.forward);
            Gizmos.DrawWireCube(attackArea.transform.position, attackArea.transform.localScale);
        }
    }
}
