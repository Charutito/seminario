using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "Game/Weapons/Factory")]
    sealed public class WeaponFactory : GameUtils.ScriptableFactory<WeaponDefinition> { }
}