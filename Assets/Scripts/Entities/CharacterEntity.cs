using BattleSystem;
using FSM;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        private CharacterFSM fsm;

        protected override void OnUpdate() 
        {
            fsm.Update();
        }

        private void Start()
        {
            fsm = new CharacterFSM(this);
        }
    }
}
