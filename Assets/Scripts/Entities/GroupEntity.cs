using BattleSystem;
using System;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityAttacker))]
    public abstract class GroupEntity : Entity, IGroup
    {
        #region Events
        public event Action<GroupAction, GroupAction> OnSetAction = delegate { };
        #endregion

        #region Properties 
        public CharacterEntity Target
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
        private CharacterEntity currentTarget = null;
        private GroupAction currentAction = GroupAction.None;
        #endregion
    }
}
