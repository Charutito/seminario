using BattleSystem;
using Stats;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityAttacker))]
    public abstract class GroupEntity : Entity, IGroup
    {
        #region Properties
        public Stat PlayerFollowRange { get { return playerFollowRange; } }
        public CharacterEntity Target { get; protected set; }
        #endregion

        #region Local Vars
        [Header("Group Config")]
        [SerializeField] private Stat playerFollowRange;
        #endregion

        #region IGroup Implementation
        public void SetTarget(CharacterEntity target)
        {
            Target = target;
        }

        public void TriggerAttack()
        {

        }

        public void TriggerSpecialAttack()
        {

        }
        #endregion
    }
}
