using System;
using System.Collections;
using Menu;
using Metadata;
using SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject selectedObject;
        public EventSystem eventSystem;
        public Text ContinueText;
        public UnityEvent OnCanContinue;

        public Animator LoadingAnimator;
        public Animator CharacterAnimator;
        public AudioSource BGM;
    
        private bool _buttonSelected;

        private bool CanContinue { get { return PlayerPrefs.HasKey(SaveKeys.LastSave); } }

        private Coroutine _musicCoroutine;


        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            LoadAfterFade(1);
        }
        
        public void ContinueGame()
        {
            if (CanContinue)
            {
                var saveData = PlayerPrefs.GetString(SaveKeys.LastSave);
                var parsedData = JsonUtility.FromJson<SavePoint.SavePointData>(saveData);
                LoadAfterFade(parsedData.SceneName);
            }
        }

        private void LoadAfterFade(int index, float delay = 2f)
        {
            _musicCoroutine = StartCoroutine(FadeMusic());
            LoadingAnimator.SetTrigger("FadeIn");
            CharacterAnimator.SetTrigger("Stand");
            FrameUtil.AfterDelay(delay, () => SceneManager.LoadScene(index));
        }
        private void LoadAfterFade(string scene, float delay = 2f)
        {
            _musicCoroutine = StartCoroutine(FadeMusic());
            LoadingAnimator.SetTrigger("FadeIn");
            CharacterAnimator.SetTrigger("Stand");
            FrameUtil.AfterDelay(delay, () => SceneManager.LoadScene(scene));
        }

        private IEnumerator FadeMusic()
        {
            while (true)
            {
                BGM.volume -= Time.deltaTime;
                yield return new WaitForSeconds(0.03f);
            }
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

        private void OnDestroy()
        {
            if (_musicCoroutine != null)
            {
                StopCoroutine(_musicCoroutine);
            }
        }

        private void Awake()
        {
            if (CanContinue)
            {
                ContinueText.color = Color.white;
                OnCanContinue.Invoke();
            }

            Cursor.lockState = CursorLockMode.Locked;
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
