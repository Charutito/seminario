
namespace Entities
{
    public class DummyEntity : GroupEntity
    {
        public override void TriggerAttack()
        {
            // Just for debug
            Animator.SetTrigger("HeavyAttack");
        }

        public override void TriggerSpecialAttack()
        {

        }

        protected override void OnUpdate()
        {

        }
    }
}
