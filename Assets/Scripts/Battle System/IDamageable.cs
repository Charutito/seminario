namespace BattleSystem
{
    public interface IDamageable : ITargettable
    {
        void TakeDamage(int damage, DamageType type = DamageType.Unknown);
    }
}
