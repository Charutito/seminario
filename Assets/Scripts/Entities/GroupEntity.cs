using BattleSystem;
using Stats;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityAttacker))]
    public abstract class GroupEntity : Entity, IGroup
    {
        #region Properties 
        public CharacterEntity Target { get; protected set; }
        #endregion

        #region Local Vars
        #endregion

        #region IGroup Implementation
        public virtual void SetTarget(CharacterEntity target)
        {
            Target = target;
        }

        public abstract void TriggerAttack();
        public abstract void TriggerSpecialAttack();
        #endregion
    }
}
