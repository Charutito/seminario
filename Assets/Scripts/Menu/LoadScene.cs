using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class LoadScene : MonoBehaviour
    {
        public void LoadByIndex(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
