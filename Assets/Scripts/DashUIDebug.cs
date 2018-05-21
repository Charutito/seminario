using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class DashUIDebug : MonoBehaviour
{
	//public Image fill;
	//public Text text;
    public List<GameObject> charges;
	
	private CharacterEntity _character;
	
	private void Start()
	{
		_character = GameManager.Instance.Character;
	}

	private void Update()
	{
        for (int i = 0; i < _character.currentDashCharges; i++)
        {
            charges[i].SetActive(true);
        }

        for (int o = _character.currentDashCharges; o < charges.Count; o++)
        {
            charges[o].SetActive(false);
        }

		//fill.fillAmount = 1 - _character.currentDashCooldown / _character.dashChargesCooldown;
	}
}
