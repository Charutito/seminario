using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SpiritUIDebug : MonoBehaviour
{
    public Image fill;
    private CharacterEntity _entity;

    private void Start()
    {
        _entity = GameManager.Instance.Character;
    }

    private void Update()
    {
        fill.fillAmount = (float)_entity.Stats.CurrentSpirit / _entity.Stats.MaxSpirit;
    }
}
