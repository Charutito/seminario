using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class DashUIDebug : MonoBehaviour
{
	public Image fill;
	public Text text;
	
	private CharacterEntity _character;
	
	private void Start()
	{
		_character = GameManager.Instance.Character;
	}

	private void Update()
	{
		text.text = _character.currentDashCharges.ToString();
		fill.fillAmount = _character.currentDashCooldown / _character.dashChargesCooldown;
	}
}
