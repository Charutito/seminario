using System.Collections.Generic;
using GameUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class PauseManager : SingletonObject<PauseManager>
    {
        public bool IsGamePaused { get; private set; }
        
        public bool IsPauseBlocked { get; private set; }

        [SerializeField]
        private GameObject pauseMenu;

        private float _originalTimeScale = -1;
        private float _originalFixedTimeScale = -1;


        public void LockPause()
        {
            IsPauseBlocked = true;
        }

        public void UnlockPause()
        {
            IsPauseBlocked = false;
        }

        public void PauseGame()
        {
            if (IsPauseBlocked || IsGamePaused) return;
            IsGamePaused = true;
            
            pauseMenu.SetActive(true);

            _originalTimeScale = Time.timeScale;
            _originalFixedTimeScale = Time.fixedDeltaTime;

            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
        }

        public void UnpauseGame()
        {
            if (IsPauseBlocked || !IsGamePaused) return;
            
            pauseMenu.SetActive(false);
            
            Time.timeScale = _originalTimeScale;
            Time.fixedDeltaTime = _originalFixedTimeScale;
            
            IsGamePaused = false;
        }

        private void OnDestroy()
        {
            if (_originalTimeScale != -1)
            {
                Time.timeScale = _originalTimeScale;
                Time.fixedDeltaTime = _originalFixedTimeScale;
            }
        }


        public void OnClickResume()
        {
            UnpauseGame();
        }
        
        public void OnClickReload()
        {
            UnpauseGame();
            GameManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void OnClickExitToMenu()
        {
            UnpauseGame();
            GameManager.Instance.LoadScene(0); // Load menu scene
        }
    }
}

