using Entities;

namespace BattleSystem
{
    public interface IGroup
    {
        CharacterEntity Target { get; set; }
        GroupAction CurrentAction { get; set; }
    }
}
