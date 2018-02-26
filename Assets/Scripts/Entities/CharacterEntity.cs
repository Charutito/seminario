using FSM;

namespace Entities
{
    public class CharacterEntity : Entity
    {
        private CharacterFSM _fsm;

        protected override void OnUpdate()
        {
            _fsm.Update();
        }

        private void Start()
        {
            _fsm = new CharacterFSM(this);
        }
    }
}
