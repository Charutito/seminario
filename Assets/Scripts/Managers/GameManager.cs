using Entities;
using GameUtils;
using Spawners;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Managers
{
    public class GameManager : SingletonObject<GameManager>
    {
        public CharacterEntity Character { get; private set; }

        public float CurrentLevelDuration
        {
            get { return Mathf.Round(Time.timeSinceLevelLoad); }
        }

        public float CurrentSessionTime
        {
            get { return Mathf.Round(Time.realtimeSinceStartup); }
        }
        
        [Header("Game End")]
        public float TimeToRestartGame = 1f;
        [Tooltip("This should be empty unless you really need it")]
        public string DeathScene = string.Empty;

        private ScreenFadeController _fadeController;

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
            if (DeathScene != string.Empty)
            {
                LoadScene(DeathScene);
            }
            else
            {
                LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
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
            
            Character = characterObject.GetComponent<CharacterEntity>();
        }

        private void Start()
        {
            Character.OnDeath += (entity) =>
            {
                _fadeController.FadeOutCanvas();
                FrameUtil.AfterDelay(TimeToRestartGame, RestartAfterDeath);
            };
        }
    }
}
