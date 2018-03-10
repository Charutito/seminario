using Entities;
using Managers;
using Steering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SkeletonFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Alerted = 1;
            public static int Arrived = 2;
            public static int TargetInSight = 3;
            public static int TargetLost = 4;
            public static int TargetInRange = 5;
            public static int GroupAlert = 6;
        }

        #region Components
        private EntityMove entityMove;
        private EntityAttacker entityAttack;
        private LineOfSight lineOfSight;
        #endregion

        #region Local Vars
        private bool isLocked = false;
        private Vector3 startingPosition;
        private Vector3 lastTargetPosition;
        #endregion

        public SkeletonFSM(SkeletonEntity e)
        {
            entityAttack = e.gameObject.GetComponent<EntityAttacker>();
            entityMove = e.gameObject.GetComponent<EntityMove>();
            lineOfSight = e.gameObject.GetComponent<LineOfSight>();

            startingPosition = e.transform.position;

            #region States Definitions
            State<int> Patrol = new State<int>("Patrolling");
            State<int> Follow = new State<int>("Following");
            State<int> Scout = new State<int>("Scouting");
            State<int> Goto = new State<int>("GointTo");
            State<int> Attack = new State<int>("Attacking");
            #endregion


            #region States Configuration
            SetInitialState(Patrol);
            
            StateConfigurer.Create(Patrol)
                .SetTransition(Trigger.None, Patrol)
                .SetTransition(Trigger.TargetInSight, Scout)
                .SetTransition(Trigger.GroupAlert, Goto);

            StateConfigurer.Create(Follow)
                .SetTransition(Trigger.TargetInSight, Follow)
                .SetTransition(Trigger.TargetLost, Scout)
                .SetTransition(Trigger.TargetInRange, Attack);

            StateConfigurer.Create(Scout)
                .SetTransition(Trigger.Alerted, Scout)
                .SetTransition(Trigger.None, Patrol)
                .SetTransition(Trigger.TargetInSight, Follow)
                .SetTransition(Trigger.GroupAlert, Goto);

            StateConfigurer.Create(Goto)
                .SetTransition(Trigger.GroupAlert, Goto)
                .SetTransition(Trigger.Arrived, Scout);

            StateConfigurer.Create(Attack)
                .SetTransition(Trigger.TargetInRange, Attack)
                .SetTransition(Trigger.TargetInSight, Follow)
                .SetTransition(Trigger.TargetLost, Scout);
            #endregion


            #region Patrol State
            var currentTimeToPatrol = 0f;

            Patrol.OnEnter += () =>
            {
                currentTimeToPatrol = 0f;
            };

            Patrol.OnUpdate += () =>
            {
                if (currentTimeToPatrol <= 0)
                {
                    var tmpPosition = startingPosition + Random.insideUnitSphere * e.patrolRadius;
                    entityMove.MoveAgent(tmpPosition);

                    currentTimeToPatrol = Random.Range(e.patrolMinWaitTime, e.patrolMaxWaitTime);
                }

                currentTimeToPatrol -= Time.deltaTime;
            };
            #endregion


            #region Follow State
            Follow.OnUpdate += () =>
            {
                lastTargetPosition = lineOfSight.Target.position;

                entityMove.RotateInstant(lastTargetPosition);
                entityMove.MoveAgent(lastTargetPosition);

                if (Vector3.Distance(lineOfSight.target.transform.position, e.transform.position) <= e.attackRange)
                {
                    Feed(Trigger.TargetInRange);
                }
            };
            #endregion


            #region Scout State
            var currentTimeToStopScout = 0f;

            Scout.OnEnter += () =>
            {
                currentTimeToStopScout = e.scoutTime;
            };

            Scout.OnUpdate += () =>
            {
                if (currentTimeToStopScout <= 0)
                {
                    Feed(Trigger.None);
                }

                /*var scoutAngle = e.scoutMaxAngle * Mathf.Sin(e.scoutSpeed * currentTimeToStopScout);

                var rotation = Quaternion.AngleAxis(Mathf.Abs(scoutAngle), Vector3.up);

                entityMove.RotateInstant(rotation * lastTargetPosition);*/

                currentTimeToStopScout -= Time.deltaTime;
            };
            #endregion


            #region Attack State
            Attack.OnEnter += () =>
            {
                isLocked = true;
                e.Animator.SetTrigger("HeavyAttack");

                entityMove.RotateInstant(lineOfSight.transform.position);
                entityAttack.LightAttack_Start();
            };
            #endregion


            #region Entity Events
            e.OnThink += () =>
            {
                if (isLocked) return;

                if (lineOfSight.TargetInSight)
                {
                    Feed(Trigger.TargetInSight);
                }
                else
                {
                    Feed(Trigger.TargetLost);
                }
            };

            e.OnAnimUnlock += () =>
            {
                isLocked = false;
            };
            #endregion
        }
    }
}

