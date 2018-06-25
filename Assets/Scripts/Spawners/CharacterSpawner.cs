using System.Collections;
using System.Collections.Generic;
using Managers;
using Metadata;
using SaveSystem;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class CharacterSpawner : MonoBehaviour
    {
        public GameObject CharacterPrefab;

        public GameObject SpawnCharacter()
        {
            var currentScene = SceneManager.GetActiveScene().name;

            var fromSave = false;
            
            if (PlayerPrefs.HasKey(string.Format(SaveKeys.LastSaveScene, currentScene)))
            {
                var serializedPosition = PlayerPrefs.GetString(string.Format(SaveKeys.LastSaveScene, currentScene));

                transform.position = JsonUtility.FromJson<SavePoint.SavePointData>(serializedPosition).Position;
                fromSave = true;
            }
            
            var data = new Dictionary<string, object>
            {
                {"current_session_time", GameManager.Instance.CurrentSessionTime},
                {"from_save", fromSave}
            };
                
            AnalyticsEvent.LevelStart(currentScene, data);
            
            var mainCamera = GameObject.FindGameObjectWithTag("GameCamera");
            mainCamera.transform.position = transform.position;

            return Instantiate(CharacterPrefab, transform.position, transform.rotation);
        }
    }
}
