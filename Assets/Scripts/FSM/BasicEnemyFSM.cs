using Entities;
using UnityEngine;

namespace FSM
{
    public class BasicEnemyFSM : EventFSM<int>
    {
        private class Trigger
        {
            public static int None = 0;
            public static int Attack = 1;
            public static int GroupAlert = 3;
        }

        #region Components
        private EntityMove entityMove;
        private EntityAttacker entityAttack;
        #endregion

        #region Local Vars
        private bool isLocked = false;
        #endregion

        public BasicEnemyFSM(GroupEntity e)
        {
            entityAttack = e.gameObject.GetComponent<EntityAttacker>();
            entityMove = e.gameObject.GetComponent<EntityMove>();

            #region States Definitions
            State<int> Stalking = new State<int>("Stalking");
            #endregion


            #region States Configuration
            SetInitialState(Stalking);
            
            StateConfigurer.Create(Stalking)
                .SetTransition(Trigger.None, Stalking);
            #endregion


            #region Patrol State

            Stalking.OnUpdate += () =>
            {
                entityMove.RotateTowards(e.Target.transform.position);

                if (Vector3.Distance(e.transform.position, e.Target.transform.position) > e.PlayerFollowRange.Max)
                {
                    e.transform.position += e.transform.forward * e.Stats.MoveSpeed.Max * Time.deltaTime;
                }

                e.transform.position += e.transform.right * e.Stats.MoveSpeed.Min * Time.deltaTime;
            };
            #endregion


            #region Follow State
            /*Follow.OnUpdate += () =>
            {
                lastTargetPosition = lineOfSight.Target.position;

                entityMove.RotateInstant(lastTargetPosition);
                entityMove.MoveAgent(lastTargetPosition);

                if (Vector3.Distance(lineOfSight.target.transform.position, e.transform.position) <= e.attackRange)
                {
                    Feed(Trigger.TargetInRange);
                }
            };*/
            #endregion


            #region Entity Events
            e.OnThink += () =>
            {
                if (isLocked || e.IsDead) return;
            };

            e.OnAnimUnlock += () =>
            {
                isLocked = false;
            };
            #endregion
        }
    }
}

