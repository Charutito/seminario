#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class DebugShortcuts : MonoBehaviour
    {
        [SerializeField] private KeyCode editorPauseKey = KeyCode.F12;

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(editorPauseKey))
            {
                EditorApplication.isPaused = true;
            }
#endif
        }
    }
}
