using UnityEditor;
using UnityEngine;

namespace BattleSystem.Abilities
{
    [CreateAssetMenu(menuName = "Battle System/Abilities/New Ability")]
    public class AbilityDefinition : ScriptableObject
    {
        #region Properties
        public string Tittle { get { return title; } }
        public string Tooltip { get { return tooltip; } }
        public Sprite Sprite { get { return sprite; } }
        public GameObject Prefab { get { return prefab; } }
        public AudioClip Sound { get { return sound; } }
        public float Cooldown { get { return cooldown; } }
        #endregion

        #region Fields
        [SerializeField]
        private string title = "New Ability";

        [SerializeField]
        private string tooltip = "Beeep BOOOOOOOP!";

        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private AudioClip sound;

        [SerializeField]
        private float cooldown = 1f;
        #endregion
    }
}