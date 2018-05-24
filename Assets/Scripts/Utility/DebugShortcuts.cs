using Managers;
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
        
        [Header("Stats")]
        [SerializeField] private KeyCode _recoverHealthCharacter = KeyCode.F1;
        [SerializeField] private KeyCode _recoverSpiritCharacter = KeyCode.F2;
        
        [Header("Save System")]
        [SerializeField] private KeyCode _clearSaveData = KeyCode.F5;


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

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(_editorPauseKey))
            {
                EditorApplication.isPaused = true;
            }
#endif
            if (Input.GetKeyDown(_recoverHealthCharacter))
            {
                RecoverHealthCharacter();
            }
            
            if (Input.GetKeyDown(_recoverSpiritCharacter))
            {
                RecoverSpiritCharacter();
            }
            
            if (Input.GetKeyDown(_clearSaveData))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log(string.Format(FormatedLog.Save, "Data deleted :( Hope you know what are you doing, bitch..."));
            }
        }
    }
}
