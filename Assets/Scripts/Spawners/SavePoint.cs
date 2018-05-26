using Managers;
using Metadata;
using UnityEngine;

namespace SaveSystem
{
    [RequireComponent(typeof(SaveGUID))]
    public class SavePoint : MonoBehaviour
    {
        private bool _isLoading;
        private SaveGUID _uniqueId;
    
        private void LoadData()
        {
            _isLoading = true;
        
            GameManager.Instance.Character.transform.position = transform.position;
            Log("Game Loaded!");
            Destroy(gameObject);
        }
    
        private void SaveData()
        {
            PlayerPrefs.SetString(SaveKeys.LastSave, _uniqueId.GameObjectId);
            PlayerPrefs.SetString(string.Format(SaveKeys.UsedSave, _uniqueId.GameObjectId), "Used");
            
            Log("Game Saved!");
            Destroy(gameObject);
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
            if (_isLoading) return;
        
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