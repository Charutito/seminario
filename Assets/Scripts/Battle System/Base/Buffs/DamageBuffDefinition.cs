using System.Linq;
using Entities;
using UnityEngine;

namespace BattleSystem.Spells
{
    [CreateAssetMenu(menuName = "Game/Spells/Debuffs/New Damage Debuff")]
    public class DamageBuffDefinition : BuffEffect
    {
        [Header("Damage")]
        public int Damage;
        public DamageType DamageType = DamageType.Spell;
        public bool IsAbsolute = false;

        protected override void ApplyEffect()
        {
            Target.TakeDamage(new Damage()
            {
                Absolute = IsAbsolute,
                Amount = Damage,
                Type = DamageType
            });
        }
    }
}
