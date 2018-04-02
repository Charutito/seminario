using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Entities;
using GameUtils;
using Metadata;
using UnityEngine;

namespace Managers
{
    public class GameManager : SingletonObject<GameManager>
    {
        public CharacterEntity Character { get; private set; }

        public Coroutine RunCoroutine(IEnumerator enumerator)
        {
            return (this == Instance) ? StartCoroutine(enumerator) : null;
        }

        private void Awake()
        {
            var characterObject = GameObject.FindGameObjectWithTag(Tags.PLAYER);
            Character = characterObject.GetComponent<CharacterEntity>();
        }
    }
}
