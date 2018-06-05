using System;
using Menu;
using Metadata;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject selectedObject;
        public EventSystem eventSystem;
        public Text ContinueText;

        public Animator LoadingAnimator;
    
        private bool _buttonSelected;
        private LoadScene _loadScene;

        private bool CanContinue { get { return PlayerPrefs.HasKey(SaveKeys.LastSave); } }


        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            LoadAfterFade(1);
        }
        
        public void ContinueGame()
        {
            if (CanContinue)
            {
                LoadAfterFade(1);
            }
        }

        private void LoadAfterFade(int index, float delay = 1.5f)
        {
            LoadingAnimator.SetTrigger("FadeIn");
            FrameUtil.AfterDelay(delay, () => _loadScene.LoadByIndex(index));
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        private void Update()
        {
            if (Math.Abs(Input.GetAxisRaw("Vertical")) > 0 && _buttonSelected == false)
            {
                eventSystem.SetSelectedGameObject(selectedObject);
                _buttonSelected = true;
            }
        }

        private void OnDisable()
        {
            _buttonSelected = false;
        }
    
        private void Awake()
        {
            if (CanContinue)
            {
                ContinueText.color = Color.white;
            }

            Cursor.lockState = CursorLockMode.Locked;
            _loadScene = GetComponent<LoadScene>();
        }

        private void OnApplicationFocus(bool pauseStatus)
        {
            if(!pauseStatus)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
