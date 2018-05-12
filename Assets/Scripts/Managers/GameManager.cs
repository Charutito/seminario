using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Entities;
using GameUtils;
using Metadata;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : SingletonObject<GameManager>
    {
        public CharacterEntity Character { get; private set; }

        public Text ComboText;
        public int Combo
        {
            get
            {
                return _combo;
            }
            set
            {
                _combo = value;
                currentTimeToReset = 0;
            }
        }
        private int _combo;
        public float timeToReset = 2f;
        private float currentTimeToReset;

        public Coroutine RunCoroutine(IEnumerator enumerator)
        {
            return (this == Instance) ? StartCoroutine(enumerator) : null;
        }

        private void Update()
        {
            if (timeToReset <= currentTimeToReset)
            {
                ResetCombo();
            }

            ComboText.gameObject.SetActive(_combo > 0);
            ComboText.text = "x" + _combo.ToString();

            currentTimeToReset += Time.deltaTime;
        }
        public void ResetCombo()
        {
            _combo = 0;
        }

        private void Awake()
        {
            var characterObject = GameObject.FindGameObjectWithTag(Tags.PLAYER);
            Character = characterObject.GetComponent<CharacterEntity>();
        }
    }
}
