using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "Game/Spells/Ability")]
    sealed public class SpellDefinition : ScriptableObject
    {
        public string title;
        public string destription;
        public SpellType type = SpellType.None;
        public Texture2D sprite;
        public GameObject prefab;
        public GameObject collisionParticle;
        public AudioClip sound;
        public float cooldown = 1f;

        public int damage = 0;
        public int projectileSpeed = 0;
    }
}