
using Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class DebugShortcuts : MonoBehaviour
    {
        [SerializeField] private KeyCode _editorPauseKey = KeyCode.F12;
        [SerializeField] private KeyCode _recoverHealthCharacter = KeyCode.F1;
        [SerializeField] private KeyCode _recoverSpiritCharacter = KeyCode.F2;


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
        }
    }
}
