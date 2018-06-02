using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LifeUIDebug : MonoBehaviour {

    public Image fill;
    private CharacterEntity _entity;

    public float timeToDisplay;
    public Animator redScreen;


    private void Start()
    {
        _entity = GameManager.Instance.Character;
        _entity.OnShowDamage += ShowRedScreen;
    }

    private void Update()
    {
        fill.fillAmount = (float)_entity.Stats.CurrentHealth / _entity.Stats.MaxHealth;    
    }

    private void ShowRedScreen()
    {
        redScreen.SetTrigger("Show");
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(timeToDisplay);
        redScreen.SetTrigger("Hide");
    }
}
