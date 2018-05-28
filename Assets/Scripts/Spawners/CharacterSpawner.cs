using System.Collections;
using System.Collections.Generic;
using Metadata;
using SaveSystem;
using UnityEngine;

namespace Spawners
{
    public class CharacterSpawner : MonoBehaviour
    {
        //public GameData SavedData; // Mockup data save (?
        public GameObject CharacterPrefab;

        public GameObject SpawnCharacter()
        {
            if (PlayerPrefs.HasKey(SaveKeys.LastSave))
            {
                var serializedPosition = PlayerPrefs.GetString(string.Format(SaveKeys.UsedSave, PlayerPrefs.GetString(SaveKeys.LastSave)));

                transform.position = JsonUtility.FromJson<SavePoint.SavePointData>(serializedPosition).Position;
            }

            return Instantiate(CharacterPrefab, transform.position, transform.rotation);
        }
    }
}
