using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class DashUIDebug : MonoBehaviour
{
    public List<Image> Charges;
	
	private CharacterEntity _character;
	
	private void Start()
	{
		_character = GameManager.Instance.Character;
	}

	private void Update()
	{
        for (var i = 0; i < Charges.Count; i++)
        {
	        var currentChange = _character.currentDashCharges;
	        
	        if (currentChange == i)
	        {
		        Charges[i].fillAmount = 1 - _character.currentDashCooldown / _character.dashChargesCooldown;
	        }
	        if (currentChange > i)
	        {
		        Charges[i].fillAmount = 1;
	        }
	        if (currentChange < i)
	        {
		        Charges[i].fillAmount = 0;
	        }
        }
	}
}
