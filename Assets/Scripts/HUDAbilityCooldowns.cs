using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class HUDAbilityCooldowns : MonoBehaviour
{
    public Image FirstAbiflityCooldown;
    public Image SecondAbilityCooldown;
    public Image ThirdAbilityCooldown;
    public Image FourthAbilityCooldown;
    
    private CharacterEntity _character;
    private void Start()
    {
        _character = GameManager.Instance.Character;
    }

    private void Update()
    {
        UpdateFirst();
        UpdateSecond();
        UpdateThird();
        UpdateFourth();
    }

    private void UpdateFirst()
    {
        FirstAbiflityCooldown.fillAmount = (_character.Stats.CurrentSpirit >= _character.FirstAbility.SpiritCost) ? 1 - _character.CurrentFirstAbilityCooldown / _character.FirstAbility.Cooldown : 0;
    }
    
    private void UpdateSecond()
    {
        SecondAbilityCooldown.fillAmount = (_character.Stats.CurrentSpirit >= _character.SecondAbility.SpiritCost) ? 1 - _character.CurrentSecondAbilityCooldown / _character.SecondAbility.Cooldown : 0;
    }

    private void UpdateThird()
    {
        ThirdAbilityCooldown.fillAmount  = (_character.Stats.CurrentSpirit >= _character.ThirdAbility.SpiritCost) ? 1 - _character.CurrentThirdAbilityCooldown / _character.ThirdAbility.Cooldown : 0;
    }
    
    private void UpdateFourth()
    {
        FourthAbilityCooldown.fillAmount = (_character.Stats.CurrentSpirit >= _character.FourthAbility.SpiritCost) ? 1 - _character.CurrentFourthAbilityCooldown / _character.FourthAbility.Cooldown : 0;
    }
}
