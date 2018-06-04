using System.Collections;
using System.Collections.Generic;
using Metadata;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class CharacterSpawner : MonoBehaviour
    {
        //public GameData SavedData; // Mockup data save (?
        public GameObject CharacterPrefab;

        public GameObject SpawnCharacter()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            
            if (PlayerPrefs.HasKey(string.Format(SaveKeys.LastSaveScene, currentScene)))
            {
                var serializedPosition = PlayerPrefs.GetString(string.Format(SaveKeys.LastSaveScene, currentScene));

                transform.position = JsonUtility.FromJson<SavePoint.SavePointData>(serializedPosition).Position;
            }
            
            var mainCamera = GameObject.FindGameObjectWithTag("GameCamera");
            mainCamera.transform.position = transform.position;

            return Instantiate(CharacterPrefab, transform.position, transform.rotation);
        }
    }
}
