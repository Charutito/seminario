#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugShortcuts : MonoBehaviour
{
    [SerializeField] private KeyCode editorPauseKey = KeyCode.F12;
    [SerializeField] private KeyCode resetScene = KeyCode.R;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(editorPauseKey))
        {
            EditorApplication.isPaused = true;
        }

        if (Input.GetKeyDown(resetScene))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
#endif
}
