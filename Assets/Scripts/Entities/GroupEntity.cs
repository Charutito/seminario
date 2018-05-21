using BattleSystem;
using System;
using Managers;
using UnityEngine;

namespace Entities
{
    public abstract class GroupEntity : Entity, IGroup
    {
        #region Events
        public event Action<GroupAction, GroupAction> OnSetAction = delegate { };
        #endregion

        #region Properties
        public ZoneController CurrentZone { get; set; }

        public Entity Target
        {
            get { return currentTarget; }
            set { currentTarget = value; }
        }

        public GroupAction CurrentAction {
            get { return currentAction; }
            set
            {
                if (value == currentAction) return;

                var lastAction = currentAction;
                currentAction = value;

                if (OnSetAction != null)
                {
                    OnSetAction(value, lastAction);
                }
            }
        }
        #endregion

        #region Local Vars
        private Entity currentTarget = null;
        private GroupAction currentAction = GroupAction.None;
        #endregion

        protected virtual void Start()
        {
            Target = GameManager.Instance.Character;
        }
    }
}
