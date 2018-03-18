using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using GameUtils;
using UnityEngine;

namespace Managers
{
    public class GameManager : SingletonObject<GameManager>
    {
        public List<WeaponDefinition> Weapons { get { return weaponFactory.Definitions; } }
        public List<SpellDefinition> Spells { get { return spellFactory.Definitions; } }

        [SerializeField] private WeaponFactory weaponFactory;
        [SerializeField] private SpellFactory spellFactory;

        public Coroutine RunCoroutine(IEnumerator enumerator)
        {
            return (this == Instance) ? StartCoroutine(enumerator) : null;
        }
    }
}
