using Managers.Camera;
using Metadata;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawners
{
    public class CharacterSpawner : MonoBehaviour
    {
        public GameObject CharacterPrefab;

        public GameObject SpawnCharacter()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            
            if (PlayerPrefs.HasKey(string.Format(SaveKeys.LastSaveScene, currentScene)))
            {
                var serializedPosition = PlayerPrefs.GetString(string.Format(SaveKeys.LastSaveScene, currentScene));

                transform.position = JsonUtility.FromJson<SavePoint.SavePointData>(serializedPosition).Position;
            }

            CameraFollow.Instance.transform.position = transform.position;

            return Instantiate(CharacterPrefab, transform.position, transform.rotation);
        }
    }
}
