using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "Game/Weapons/Weapon")]
    sealed public class WeaponDefinition : ScriptableObject
    {
        public string title;
        public string destription;
        public WeaponType type = WeaponType.None;
        public Texture2D sprite;
        public GameObject prefab;
        public GameObject collisionParticle;
        public AudioClip sound;
        public float cooldown = 1f;
        public int damage = 0;
    }
}