using Managers;
using Menu;
using Metadata;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utility
{
    public class DebugShortcuts : MonoBehaviour
    {
        [Header("Editor")]
        [SerializeField] private KeyCode _editorPauseKey = KeyCode.F12;

        [SerializeField] private KeyCode _toggleMouse = KeyCode.Mouse3;
        
        [Header("Stats")]
        [SerializeField] private KeyCode _recoverHealthCharacter = KeyCode.F1;
        [SerializeField] private KeyCode _recoverSpiritCharacter = KeyCode.F2;
        [SerializeField] private KeyCode _characterNoClip = KeyCode.F3;
        
        [Header("Scenes")]
        [SerializeField] private KeyCode _reloadScene = KeyCode.F5;
        [SerializeField] private KeyCode _loadMenu = KeyCode.F6;
        [SerializeField] private KeyCode _loadGame = KeyCode.F7;
        [SerializeField] private KeyCode _loadTest = KeyCode.F8;
        
        [Header("Save System")]
        [SerializeField] private KeyCode _clearSaveData = KeyCode.F9;

        private LoadScene  _loadScene;
        private bool _cursorLocked = true;

        public void RecoverHealthCharacter()
        {
            var character = GameManager.Instance.Character;
            character.Heal(character.Stats.Health.Max);
        }
        
        public void RecoverSpiritCharacter()
        {
            var character = GameManager.Instance.Character;
            character.HealEnergy(character.Stats.Spirit.Max);
        }
        
        public void ToggleCharacterNoClip()
        {
            var character = GameManager.Instance.Character;
            character.Agent.enabled = !character.Agent.enabled;
        }

        private void Awake()
        {
            _loadScene = GetComponent<LoadScene>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(_editorPauseKey))
            {
                EditorApplication.isPaused = true;
            }
#endif
#if DEBUG
            if (Input.GetKeyDown(_toggleMouse))
            {
                Cursor.lockState = _cursorLocked ? CursorLockMode.Locked : CursorLockMode.Confined;
            }
#endif
            
            // Stats
            if (Input.GetKeyDown(_recoverHealthCharacter))
            {
                RecoverHealthCharacter();
            }
            if (Input.GetKeyDown(_recoverSpiritCharacter))
            {
                RecoverSpiritCharacter();
            }
            if (Input.GetKeyDown(_characterNoClip))
            {
                ToggleCharacterNoClip();
            }
            
            // Scenes
            if (Input.GetKeyDown(_reloadScene))
            {
                _loadScene.ReloadScene();
            }
            if (Input.GetKeyDown(_loadMenu))
            {
                _loadScene.LoadByIndex(0);
            }
            if (Input.GetKeyDown(_loadGame))
            {
                _loadScene.LoadByIndex(1);
            }
            if (Input.GetKeyDown(_loadTest))
            {
                _loadScene.LoadByIndex(2);
            }

            // Save System
            if (Input.GetKeyDown(_clearSaveData))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log(string.Format(FormatedLog.Save, "Data deleted :( Hope you know what are you doing, bitch..."));
            }
        }
    }
}
