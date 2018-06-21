using System.Collections;
using Entities;
using GameUtils;
using Metadata;
using Spawners;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

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
        
        [Header("Game End")]
        public float TimeToRestartGame = 1.5f;

        private ScreenFadeController _fadeController;
        
        private float _currentTimeToReset;
        private int _combo;
        
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

        public void LoadScene(string sceneName)
        {
            _fadeController.FadeOut();
            
            FrameUtil.AfterDelay(TimeToRestartGame, () => SceneManager.LoadScene(sceneName));
        }
        
        public void LoadScene(int sceneIndex)
        {
            _fadeController.FadeOut();
            
            FrameUtil.AfterDelay(TimeToRestartGame, () => SceneManager.LoadScene(sceneIndex));
        }

        private void RestartAfterDeath()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Awake()
        {
            Time.timeScale = 1;
            
            _fadeController = GetComponentInChildren<ScreenFadeController>();
                
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

        private void Start()
        {
            Character.OnDeath += (entity) => FrameUtil.AfterDelay(TimeToRestartGame, RestartAfterDeath);
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
