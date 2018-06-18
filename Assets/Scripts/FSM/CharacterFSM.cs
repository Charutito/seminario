using Entities;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem;
using BattleSystem.Spells;
using UnityEngine;
using Util;
using GameUtils;
using InputManager = Managers.InputManager;

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
            public static int SpiritPunch = 8;
            public static int GettingHitBack = 9;
            public static int DancingBlades = 10;
            public static int Backflip = 11;
            public static int Fireball = 12;
        }

        private CharacterEntity entity;

        public CharacterFSM(CharacterEntity entity)
        {
            this.debugName = "CharacterFSM";
            this.entity = entity;
            
            var Idle = new State<int>("Idle");
            var Moving = new State<int>("Moving");
            var Attacking = new State<int>("Light Attacking");
			var SpecialAttack = new State<int>("Special Attacking");
            var ChargedAttack = new State<int>("Charged Attacking");
            var Stunned  = new State<int>("Stunned");
            var Dead  = new State<int>("Dead");
            var GetHit = new State<int>("GetHit");
            var GettingHitBack = new State<int>("Getting Hit Back");
            var SpiritPunch = new State<int>("SpiritPunch");
            var BackFlip = new State<int>("BackFlip");
            var Fireball = new State<int>("Fireball");
            var DancingBlades = new State<int>("DancingBlades");

            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Moving)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.ChargedAttack, ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Attacking)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(SpecialAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(ChargedAttack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle);

            StateConfigurer.Create(GetHit)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(GettingHitBack)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(SpiritPunch)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);
            
            StateConfigurer.Create(DancingBlades)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);
            
            StateConfigurer.Create(Fireball)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.Backflip, BackFlip)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);
            
            StateConfigurer.Create(BackFlip)
                .SetTransition(Trigger.Attack, Attacking)
                .SetTransition(Trigger.SpecialAttack, SpecialAttack)
                .SetTransition(Trigger.Move, Moving)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Dead)
                .SetTransition(Trigger.None, Idle)
                .SetTransition(Trigger.SpiritPunch, SpiritPunch)
                .SetTransition(Trigger.Fireball, Fireball)
                .SetTransition(Trigger.DancingBlades, DancingBlades)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);


            #region Character Events
            entity.OnAttack += FeedAttack;
            entity.OnDash += DoDash;
            entity.OnSpecialAttack += FeedSpecialAttack;
            entity.OnStun += FeedStun;
            entity.OnMove += FeedMove;
            entity.OnGetHit += FeedGetHit;
            entity.OnGettingHitBack += FeedGettingHitBack;
            entity.OnSpiritPunch += FeedSpiritPunch;
            entity.OnDancingBlades += FeedDancingBlades;
            entity.OnFireballCast += FeedFireball;
            entity.OnBackflipCast += FeedBackflip;

            entity.OnAttackRecovering += () => {
				entity.IsAttacking = false;
			    entity.IsSpecialAttacking = false;
			};
            entity.OnAttackRecovered += () => {
                entity.IsAttacking = false;
                entity.IsSpecialAttacking = false;
				Feed(Trigger.None);
            };
            entity.OnDeath += (e) =>
            {
                entity.OnAttack -= FeedAttack;
                entity.OnDash -= DoDash;
                entity.OnSpecialAttack -= FeedSpecialAttack;
                entity.OnStun -= FeedStun;
                entity.OnGetHit -= FeedGetHit;
                entity.OnMove -= FeedMove;
                entity.OnGettingHitBack -= FeedGettingHitBack;
                entity.OnSpiritPunch -= FeedSpiritPunch;
                entity.OnDancingBlades -= FeedDancingBlades;
                entity.OnFireballCast -= FeedFireball;
                entity.OnBackflipCast -= FeedBackflip;
                Feed(Trigger.Die);
            };
            #endregion

            #region Moving
            Moving.OnEnter += () =>
            {
                entity.Animator.SetFloat("Velocity Z", 1);
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
            };
            #endregion

            #region Light Attack
            Attacking.OnEnter += () =>
            {
				entity.IsAttacking = true;
                
                entity.OnMove -= FeedMove;
                entity.EntityMove.RotateInstant(entity.EntityAttacker.lineArea.HitCollider.transform.position);
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
                entity.EntityMove.RotateInstant(entity.EntityAttacker.lineArea.HitCollider.transform.position);
                entity.EntityAttacker.HeavyAttack_Start();
            };

            SpecialAttack.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion
            
            #region Stunned State
            Stunned.OnEnter += () =>
            {
                entity.Animator.SetTrigger("Countered");
                entity.IsAttacking = false;
                entity.IsSpecialAttacking = false;
            };
            #endregion

            #region GetHit State
            GetHit.OnEnter += () =>
            {
                entity.Animator.SetTrigger("GetHit");
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;
                entity.IsAttacking = false;
                entity.IsSpecialAttacking = false;
            };
            #endregion

            #region Spirit Punch Spell
            SpiritPunch.OnEnter += () =>
            {
                entity.IsSpecialAttacking = true;
                entity.OnMove -= FeedMove;
                entity.Animator.SetTrigger("SpiritPunch");
                entity.SecondAbilityHit();
            };

            SpiritPunch.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion
            
            #region Dancing Blades Spell
            DancingBlades.OnEnter += () =>
            {
                entity.IsSpecialAttacking = true;
                entity.OnMove -= FeedMove;
                entity.EntityMove.RotateInstant(entity.EntityAttacker.lineArea.HitCollider.transform.position);
                entity.Animator.SetTrigger("ChargedAttack");
            };

            DancingBlades.OnExit += () =>
            {
                entity.OnMove += FeedMove;
            };
            #endregion
            
            #region FireballSpell
            
            Fireball.OnEnter += () =>
            {
                entity.CurrentTimeToCastFirstAbility = entity.FirstAbility.AutoCastTime;
                entity.IsSpecialAttacking = true;
                entity.OnMove -= FeedMove;
                entity.Animator.SetBool("AimSpell", true);
            };

            Fireball.OnUpdate += () =>
            {
                entity.CurrentTimeToCastFirstAbility -= Time.deltaTime;
                
                if (!InputManager.Instance.FirstAbility || entity.CurrentTimeToCastFirstAbility <= 0)
                {
                    Feed(Trigger.None);
                    return;
                }

                if (InputManager.Instance.AxisMoving)
                {
                    entity.EntityMove.RotateTowards(entity.EntityAttacker.lineArea.HitCollider.transform.position);
                }
            };

            Fireball.OnExit += () =>
            {
                entity.Animator.SetBool("AimSpell", false);
                entity.Animator.SetFloat("Velocity Z", 0f);
                entity.IsSpecialAttacking = false;
                entity.FirstAbilityHit();
                entity.OnMove += FeedMove;
            };
            #endregion
            
            #region Backflip Spell
            BackFlip.OnEnter += () =>
            {
                entity.IsSpecialAttacking = true;
                entity.OnMove -= FeedMove;
                entity.Animator.SetTrigger("BackFlip");
                entity.FourthAbilityHit();
            };

            BackFlip.OnExit += () =>
            {
                entity.OnMove += FeedMove;
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
            // Prevents character dash outside moving or idle state. (Current is the current FSM state)
            if (!string.Equals(Current.name, "Moving") &&
                !string.Equals(Current.name, "Light Attacking") &&
                !string.Equals(Current.name, "Idle")) return;

            var dashLength = entity.dashLenght;

            if (entity.currentDashCharges <= 0)
            {
                dashLength = 1;
                entity.currentDashCooldown = entity.dashChargesCooldown;
            }
            else
            {
                entity.vfxManager.Dash();
                entity.currentDashCharges--;
                entity.Animator.SetTrigger("TriggerDash");
            }
            
            var dashPosition =  entity.transform.position +
                               entity.EntityAttacker.lineArea.transform.forward * entity.transform.GetMaxDistance(entity.EntityAttacker.lineArea.transform.forward, dashLength);
            
            entity.EntityMove.RotateInstant(dashPosition);
            entity.EntityMove.SmoothMoveTransform(dashPosition, 0.1f);
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

        private void FeedSpiritPunch()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.SpiritPunch);
            }
        }

        private void FeedSpecialAttack()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.SpecialAttack);
            }
        }
        
        private void FeedDancingBlades()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking && entity.EntityAttacker.lineArea.GetEnemiesInSight().Any())
            {
                Feed(Trigger.DancingBlades);
            }
        }
        
        private void FeedFireball()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.Fireball);
            }
        }
        
        private void FeedBackflip()
        {
            if (!entity.IsAttacking && !entity.IsSpecialAttacking)
            {
                Feed(Trigger.Backflip);
            }
        }
        #endregion
    }
}

