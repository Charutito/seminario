using BattleSystem;
using Entities;
using UnityEngine;
using Util;

namespace FSM
{
    public class BasicEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int Stalking = 2;
            public static int Die = 3;
            public static int Stun = 4;
            public static int GetHit = 5;
            public static int GettingHitBack = 6;
        }

        /// <summary>
        /// Variables and triggers in animator
        /// </summary>
        private class Animations
        {
            public static string Attack         = "Attack";
            public static string SpecialAttack  = "SpecialAttack";
            public static string Death          = "Death";
            public static string RandomDeath    = "RandomDeath";
            public static string Countered      = "Countered";
            public static string Move           = "Velocity Z";
            public static string GetHit         = "GetHit";
            public static string GettingHitBack = "GetHitBack";

        }
        
        #region Components
        private BasicEnemy entity;
        #endregion

        #region Local Vars
        private string attackAnimation = string.Empty;
        private int currentHitsToStun = 0; // Tendria que reducirse a lo largo del tiempo si no recibe ataques
        #endregion

        public BasicEnemyFSM(BasicEnemy entity)
        {
            this.debugName = "BasicFSM";
            this.entity = entity;
            #region States Definitions
            State<int> Idle = new State<int>("Idling");
            State<int> Stalk = new State<int>("Stalking");
            State<int> Follow = new State<int>("Following");
            State<int> Attack = new State<int>("Attacking");
            State<int> Death = new State<int>("Death");
            State<int> Stunned = new State<int>("Stunned");
            State<int> GetHit = new State<int>("GetHit");
            State<int> GettingHitBack = new State<int>("Getting Hit Back");
            #endregion


            #region States Configuration
            SetInitialState(Idle);

            StateConfigurer.Create(Idle)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Stalk)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Follow)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.Attack, Attack)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Stun, Stunned)
                .SetTransition(Trigger.Die, Death)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);


            StateConfigurer.Create(Stunned)
                .SetTransition(Trigger.Attack, Follow)
                .SetTransition(Trigger.Stalking, Stalk)
                .SetTransition(Trigger.Die, Death)
                .SetTransition(Trigger.GetHit, GetHit)
                .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(GetHit)
               .SetTransition(Trigger.Attack, Attack)
               .SetTransition(Trigger.Stalking, Stalk)
               .SetTransition(Trigger.Die, Death)
               .SetTransition(Trigger.GetHit, GetHit)
               .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            StateConfigurer.Create(GettingHitBack)
               .SetTransition(Trigger.Attack, Attack)
               .SetTransition(Trigger.Stalking, Stalk)
               .SetTransition(Trigger.Die, Death)
               .SetTransition(Trigger.GetHit, GetHit)
               .SetTransition(Trigger.GettingHitBack, GettingHitBack);

            #endregion

            #region Stalk State 
            var nextLocation = Vector3.zero;
            var normalSpeed = 1f;

            Stalk.OnEnter += () =>
            {
                normalSpeed = entity.Agent.speed;

                entity.Agent.speed = 0.3f;
            };

            Stalk.OnUpdate += () =>
            {
                if (entity.EntityMove.HasAgentArrived())
                {
                    var newLocation = Random.insideUnitCircle * 4;
                    
                    nextLocation = entity.transform.position + new Vector3(newLocation.x, entity.transform.position.y, newLocation.y);
                    
                    entity.EntityMove.MoveAgent(nextLocation);
                }
                
                entity.EntityMove.RotateInstant(entity.Target.transform.position);
            };
            
            Stalk.OnExit += () =>
            {
                entity.Agent.isStopped = true;
                entity.Agent.speed = normalSpeed;
            };
            #endregion

            #region Follow State
            Follow.OnEnter += () =>
            {
                entity.Animator.SetFloat(Animations.Move, 1);
            };

            Follow.OnUpdate += () =>
            {
                entity.EntityMove.RotateInstant(entity.Target.transform.position);
                entity.EntityMove.MoveAgent(entity.Target.transform.position);

                if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) <= entity.AttackRange)
                {
                    Feed(Trigger.Attack);
                }
            };

            Follow.OnExit += () =>
            {
                entity.Animator.SetFloat(Animations.Move, 0);
            };
            #endregion


            #region Attack State
            Attack.OnEnter += () =>
            {
                entity.Animator.SetTrigger(attackAnimation);
            };
            #endregion

            #region Death State
            Death.OnEnter += () =>
            {
                entity.Animator.SetBool("Death", false);
                entity.Animator.SetTrigger("TriggerDeath");
                entity.Animator.SetInteger("RandomDeath", Random.Range(0, 3));

                entity.Agent.enabled = false;
                entity.Collider.enabled = false;
            };
            #endregion

            #region Stunned State
            Stunned.OnEnter += () =>
            {
				currentHitsToStun = 0;

                entity.Animator.SetTrigger(Animations.Countered);

                FrameUtil.AfterDelay(entity.stunDuration, () =>
                {
                    Feed(Trigger.Stalking);
                });
            };
            #endregion

            #region GetHit State

            GetHit.OnEnter += () =>
            {
                entity.Animator.SetTrigger(Animations.GetHit);
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;

                FrameUtil.AfterDelay(entity.getHitDuration, () =>
                {
                    Feed(Trigger.Attack);
                });
            };

            #endregion

            #region GettingHitBack State

            GettingHitBack.OnEnter += () =>
            {
                entity.Animator.SetTrigger(Animations.GettingHitBack);
                entity.GetComponent<EntityAttacker>().attackArea.enabled = false;
                entity.HitFeedback();

                FrameUtil.AfterDelay(entity.getHitBackDuration, () =>
                {
                    Feed(Trigger.Attack);
                });
            };

            #endregion

            #region Entity Events
            entity.OnAttackRecovered += OnAttackRecover;
            entity.OnSetAction += OnSetAction;
            entity.OnTakeDamage += OnTakingDamage;

            entity.OnDeath += (e) =>
            {
				entity.OnAttackRecovered -= OnAttackRecover;
                entity.OnSetAction -= OnSetAction;
                entity.OnTakeDamage -= OnTakingDamage;
                Feed(Trigger.Die);
            };
            #endregion
        }
        
        private void OnAttackRecover()
        {
            entity.IsAttacking = false;
            entity.CurrentAction = GroupAction.Stalking;
        }

        private void OnTakingDamage(int damage, DamageType type)
        {
            entity.HitFeedback();
            
            currentHitsToStun++;

            if (type == DamageType.Block || currentHitsToStun >= entity.hitsToGetStunned)
            {
                Feed(Trigger.Stun);
            }else if(type == DamageType.ThirdAttack)
            {
                Feed(Trigger.GettingHitBack);
            }else
            {
                Feed(Trigger.GetHit);
            }
        }

        private void OnSetAction(GroupAction newAction, GroupAction lastAction)
        {
            if (newAction == GroupAction.Attacking)
            {
                attackAnimation = Animations.Attack;
                entity.IsAttacking = true;
                Feed(Trigger.Attack);
            }
            else if (newAction == GroupAction.SpecialAttack)
            {
                attackAnimation = Animations.SpecialAttack;
                Feed(Trigger.Attack);
            }
            else if (newAction == GroupAction.Stalking)
            {
                Feed(Trigger.Stalking);
            }
        }
    }
}

