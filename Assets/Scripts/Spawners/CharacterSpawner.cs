using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class CharacterSpawner : MonoBehaviour
    {
        //public GameData SavedData; // Mockup data save (?
        public GameObject CharacterPrefab;

        public GameObject SpawnCharacter()
        {
            return Instantiate(CharacterPrefab, transform.position, transform.rotation);
        }
    }
}
