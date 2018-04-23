using Entities;

namespace BattleSystem
{
    public interface IGroup
    {
        Entity Target { get; set; }
        GroupAction CurrentAction { get; set; }
    }
}
