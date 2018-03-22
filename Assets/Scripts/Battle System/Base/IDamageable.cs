namespace BattleSystem
{
    public interface IDamageable
    {
        void TakeDamage(int damage, DamageType type = DamageType.Unknown);
    }
}
