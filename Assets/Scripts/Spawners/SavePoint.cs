using System;
using Managers;
using Metadata;
using UnityEngine;
using UnityEngine.Events;

namespace SaveSystem
{
    [RequireComponent(typeof(SaveGUID))]
    public class SavePoint : MonoBehaviour
    {
        [Serializable]
        public class SavePointData
        {
            public Vector3 Position;
        }

        public UnityEvent OnSaveEvent;
        public UnityEvent OnLoadEvent;

        private bool _isUsed;
        private SaveGUID _uniqueId;

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void LoadData()
        {
            _isUsed = true;
        
            //GameManager.Instance.Character.transform.position = transform.position;
            
            OnLoadEvent.Invoke();
            
            Log("Game Loaded!");
            //Destroy(gameObject);
        }
    
        private void SaveData()
        {
            _isUsed = true;
            
            var dataToSave = new SavePointData { Position = transform.position };
            PlayerPrefs.SetString(SaveKeys.LastSave, _uniqueId.GameObjectId);
            PlayerPrefs.SetString(string.Format(SaveKeys.UsedSave, _uniqueId.GameObjectId), JsonUtility.ToJson(dataToSave));
            
            OnSaveEvent.Invoke();
            Log("Game Saved!");
            // Destroy(gameObject);
        }

        private void Awake()
        {
            _uniqueId = GetComponent<SaveGUID>();
        }

        private void Start()
        {
            if (PlayerPrefs.GetString(SaveKeys.LastSave) == _uniqueId.GameObjectId)
            {
                LoadData();
            }
            else if (PlayerPrefs.HasKey(string.Format(SaveKeys.UsedSave, _uniqueId.GameObjectId)))
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isUsed) return;
        
            SaveData();
        }
        
        private static void Log(string message)
        {
#if DEBUG
            Debug.Log(string.Format(FormatedLog.Save, message));
#endif
        }
    }
}