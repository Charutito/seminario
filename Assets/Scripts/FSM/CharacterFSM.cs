using Entities;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;
using Util;

namespace FSM
{
    public class CharacterFSM : EventFSM<int>
    {
        private static class Trigger
        {
            public static int Move = 0;
            public static int Attack = 1;
            public static int SpecialAttack = 2;
            public static int ChargedAttack = 3;
            public static int Stun = 4;
            public static int None = 5;
            public static int Die = 6;
            public static int GetHit = 7;
            public static int AimSpell = 8;
            public static int GettingHitBack = 9;
        }

        private CharacterEntity entity;

        public CharacterFSM(CharacterEntity entity)
        {
            this.debugName = "CharacterFSM";
            this.entity = entity;
            
            State<int> Idle = new State<int>("Idle");
            State<int> Moving = new State<int>("Moving");
            State<int> Dashing = new State<int>("Dash");
			State<int> Attacking = new State<int>("Light Attacking");
			State<int> SpecialAttack = new State<int>("Special Attacking");
			State<int> ChargedAttack = new State<int>("Charged Attacking");
			State<int> CastingSpell = new State<int>("Casting Spell");
            State<int> Stunned  = new State<int>("Stunned");
            State<int> Dead  = new State<int>("Dead");
            State<int> GetHit = new State<int>("GetHit");
            State<int> GettingHitBack = new State<int>("Getting Hit Back");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.AimSpell, CastingSpell)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Moving)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.AimSpell, CastingSpell)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Attacking)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.AimSpell, CastingSpell)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(SpecialAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.AimSpell, CastingSpell)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.AimSpell, CastingSpell)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(CastingSpell)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle);

            StateConfigurer.Create(GetHit)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(GettingHitBack)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);


            #region Character Events
            entity.OnAttack += FeedAttack;
            entity.OnDash += DoDash;
            entity.OnSpecialAttack += FeedSpecialAttack;
            entity.OnStun += FeedStun;
            entity.OnMove += FeedMove;
            entity.OnGetHit += FeedGetHit;
            entity.OnSpellAiming += FeedAimSpell;
            entity.OnGettingHitBack += FeedGettingHitBack;


            entity.OnAttackRecovering += () => {
				entity.IsAttacking = false;
			    entity.IsSpecialAttacking = false;
			};
            entity.OnAttackRecovered += () => {
				Feed(Trigger.None);
            };
            entity.OnDeath += (e) =>
            {
                entity.OnAttack -= FeedAttack;
                entity.OnDash -= DoDash;
                entity.OnSpecialAttack -= FeedSpecialAttack;
                entity.OnStun -= FeedStun;
                entity.OnSpellAiming -= FeedAimSpell;
                entity.OnGetHit -= FeedGetHit;
                entity.OnMove -= FeedMove;
                entity.OnGettingHitBack -= FeedGettingHitBack;
                Feed(Trigger.Die);
            };
            #endregion


            #region Moving
            Moving.OnEnter += () =>
            {
                entity.Stats.MoveSpeed.Current = entity.Stats.MoveSpeed.Max;
                entity.Animator.SetFloat("Velocity Z", 1);
                entity.OnDash += DoDash;
            };

            Moving.OnUpdate += () =>
            {
                if (InputManager.Instance.AxisMoving)
                {
                    entity.EntityMove.MoveTransform(InputManager.Instance.AxisHorizontal, InputManager.Instance.AxisVertical);
                }
                else
                {
                    Feed(Trigger.None);
                }
            };

            Moving.OnExit += () =>
            {
                entity.Animator.SetFloat("Velocity Z", 0);
                entity.OnDash -= DoDash;
            };
            #endregion

            #region Light Attack
            Attacking.OnEnter += () =>
            {
				entity.IsAttacking = true;

                entity.OnMove -= FeedMove;
                entity.EntityAttacker.LightAttack_Start();
            };

            Attacking.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion

            #region Getting Hit Back
            GettingHitBack.OnEnter += () =>
            {
                entity.Animator.SetTrigger("GetHitBack");
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;
            };
            #endregion

            #region Special Attack
            SpecialAttack.OnEnter += () =>
            {
                entity.IsSpecialAttacking = true;

                entity.OnMove -= FeedMove;
                entity.EntityAttacker.HeavyAttack_Start();
            };

            SpecialAttack.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion
            
            #region Casting Spell
            var canShoot = true;
            var preparingShoot = false;
            var currentCastTime = 0f;
            
            CastingSpell.OnEnter += () =>
            {
                entity.OnMove -= FeedMove;
                entity.OnDash -= DoDash;
                currentCastTime = 0;
                entity.Animator.SetBool("AimSpell", true);
            };
            
            CastingSpell.OnUpdate += () =>
            {
                if (InputManager.Instance.AxisMoving)
                {
                    var newRotation = entity.transform.position + new Vector3(InputManager.Instance.AxisHorizontal, 0, InputManager.Instance.AxisVertical);
                    entity.EntityMove.RotateTowards(newRotation);
                }

                if (!InputManager.Instance.AbilityAim)
                {
                    Feed(Trigger.None);
                }
                else if (InputManager.Instance.AbilityCast &&
                         canShoot &&
                         entity.Stats.Spirit.Current < entity.fireballSpell.SpiritCost)
                {
                    canShoot = false;
                    entity.noSpiritSound.Play();
                }
                else if(InputManager.Instance.AbilityCast &&
                        canShoot &&
                        entity.maxChargeTime > currentCastTime)
                {
                    preparingShoot = true;
                }
                else if(preparingShoot && canShoot)
                {
                    canShoot = false;
                    
                    if (currentCastTime <= entity.minChargeTime)
                    {
                        entity.Stats.Spirit.Current -= entity.fireballSpell.SpiritCost;
                        SpellDefinition.Cast(entity.fireballSpell, entity.castPosition, entity.transform.rotation);
                        entity.Animator.SetTrigger("Shoot");
                    }
                    else
                    {
                        SpellDefinition.Cast(entity.chargedFireballSpell, entity.castPosition, entity.transform.rotation);
                        entity.Animator.SetTrigger("Shoot");
                    }
                    
                    currentCastTime = 0;
                }
                else if(!InputManager.Instance.AbilityCast)
                {
                    canShoot = true;
                    preparingShoot = false;
                    preparingShoot = false;
                    currentCastTime = 0;
                }

                if (preparingShoot)
                {
                    currentCastTime += Time.deltaTime;
                }
            };

            CastingSpell.OnExit += () =>
            {
                canShoot = true;
                preparingShoot = false;
                
                entity.OnMove += FeedMove;
                entity.OnDash += DoDash;
                entity.Animator.SetBool("AimSpell", false);
            };
            #endregion
            
            
            #region Stunned State
            Stunned.OnEnter += () =>
            {
                entity.Animator.SetTrigger("Countered");
            };
            #endregion

            #region GetHit State
            GetHit.OnEnter += () =>
            {
                entity.Animator.SetTrigger("GetHit");
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;
            };
            #endregion

            #region Dead State
            Dead.OnEnter += () =>
            {
                entity.Animator.SetBool("Death", true);
                entity.Animator.SetTrigger("TriggerDeath");
            };
            #endregion
        }

        private void DoDash()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking && entity.currentDashCharges > 0)
            {
                var dashPosition = entity.transform.position +
                                   entity.EntityAttacker.lineArea.transform.forward * entity.dashLenght;
                
                entity.currentDashCharges--;
                entity.EntityMove.RotateInstant(dashPosition);
                entity.EntityMove.SmoothMoveTransform(dashPosition, 0.1f);
            }
        }

        #region Feed Functions
        private void FeedStun()
        {
            Feed(Trigger.Stun);
        }
        
        private void FeedMove()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.Move);
            }
        }

        private void FeedGetHit()
        {
            Feed(Trigger.GetHit);
        }

        private void FeedGettingHitBack()
        {
            Feed(Trigger.GettingHitBack);
        }

        private void FeedAttack()
        {
			if (!entity.IsAttacking && !entity.IsSpecialAttacking)
			{
				Feed(Trigger.Attack);
			}
        }

        private void FeedSpecialAttack()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.SpecialAttack);
            }
        }
        
        private void FeedAimSpell()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.AimSpell);
            }
        }
        #endregion
    }
}

