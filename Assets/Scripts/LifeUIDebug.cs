using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LifeUIDebug : MonoBehaviour {

    public Image fill;

    private CharacterEntity _character;

    private void Start()
    {
        _character = GameManager.Instance.Character;
    }

    private void Update()
    {
        fill.fillAmount = _character.Stats.Health.Current / _character.Stats.Health.Max;    
    }
}
