using Entities;

namespace BattleSystem
{
    public interface IGroup
    {
        void SetTarget(CharacterEntity target);
        void TriggerAttack();
        void TriggerSpecialAttack();
    }
}
