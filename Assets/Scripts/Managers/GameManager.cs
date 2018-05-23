using System.Collections;
using Entities;
using GameUtils;
using Metadata;
using Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : SingletonObject<GameManager>
    {
        public CharacterEntity Character { get; private set; }
        public int Combo
        {
            get { return _combo; }
            set
            {
                _combo = value;
                _currentTimeToReset = 0;
            }
        }
        
        public Text ComboText;
        public float TimeToReset = 2f;
        
        private float _currentTimeToReset;
        private int _combo;

        public Coroutine RunCoroutine(IEnumerator enumerator)
        {
            return (this == Instance) ? StartCoroutine(enumerator) : null;
        }
        
        public void ResetCombo()
        {
            _combo = 0;
        }

        public static void EditorStop(string message)
        {
            #if UNITY_EDITOR
            Debug.LogWarning("[HALT] " + message);
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        private void Awake()
        {
            var characterSpawner = FindObjectOfType<CharacterSpawner>();

            
            #if UNITY_EDITOR
            if (characterSpawner == null)
            {
                EditorStop("Character Spawn not found in scene. Please add one.");
                return;
            }
            #endif
            
            var characterObject = characterSpawner.SpawnCharacter();
            
            #if UNITY_EDITOR
            if (characterObject == null)
            {
                EditorStop("Spawn of character failed, please check your Save Data or Spawn Object");
                return;
            }
            #endif
            
            //var characterObject = GameObject.FindGameObjectWithTag(Tags.PLAYER);
            Character = characterObject.GetComponent<CharacterEntity>();
        }
        
        private void Update()
        {
            if (TimeToReset <= _currentTimeToReset)
            {
                ResetCombo();
            }

            ComboText.gameObject.SetActive(_combo > 0);
            ComboText.text = "x" + _combo.ToString();

            _currentTimeToReset += Time.deltaTime;
        }
    }
}
