using BattleSystem.Spells;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "Game/Spells/Ability")]
    sealed public class SpellDefinition : ScriptableObject
    {
        [Header("UI")]
        public string Name = string.Empty;
        public string Description = string.Empty;
        public Texture2D Icon;
        
        [Header("Cast")]
        public GameObject Prefab;
        public GameObject SubCast;
        public GameObject HitEffect;
        public GameObject DeathEffect;
        public float Cooldown = 1f;
        public int SpiritCost;
        
        [Header("Damage")]
        public int Damage;
        public int DamageMultiplier;
        public DamageType DamageType = DamageType.Spell;
        
        [Header("Sounds")]
        public AudioEvent CastSound;
        public AudioEvent DeathSound;
        
        [Header("Projectile")]
        public float Speed = 0f;
        public float DestroyAfterTime = 1f;
        public bool DestroyOnCollision = true;

        [Header("AoE")]
        public float EffectRadius = 1f;
        public int MaxAffected = 5;
        public float WaitTime = 0.1f;
        public LayerMask EffectLayer;
        
        
        public static void Cast(SpellDefinition definition, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var newSpell = Instantiate(definition.Prefab, position, rotation, parent);

            var spellComponent = newSpell.GetComponent<SpellBehaviour>();
            
            if (spellComponent != null)
            {
                spellComponent.Definition = definition;

                if (definition.CastSound != null)
                {
                    definition.CastSound.PlayAtPoint(position);
                }
            }
        }

        public static void Cast(SpellDefinition definition, Transform origin, bool originIsParent = false)
        {
            Cast(definition, origin.position, origin.rotation, originIsParent ? origin : null);
        }

        public static void CastChild(SpellDefinition definition, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var newSpell = Instantiate(definition.SubCast, position, rotation, parent);

            var spellComponent = newSpell.GetComponent<SpellBehaviour>();
            
            if (spellComponent != null)
            {
                spellComponent.Definition = definition;
            }
        }
    }
}