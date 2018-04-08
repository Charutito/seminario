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
        _entity.OnTakeDamage += ShowRedScreen;
    }

    private void Update()
    {
        fill.fillAmount = _entity.Stats.Health.Current / _entity.Stats.Health.Max;    
    }

    private void ShowRedScreen()
    {
        redScreen.SetTrigger("Show");
        //redScreen.SetActive(true);
        StartCoroutine(Show(timeToDisplay));
    }

    private IEnumerator Show(float timeToDisplay)
    {
        yield return new WaitForSeconds(timeToDisplay);
        //redScreen.SetActive(false);
        redScreen.SetTrigger("Hide");
    }
}
