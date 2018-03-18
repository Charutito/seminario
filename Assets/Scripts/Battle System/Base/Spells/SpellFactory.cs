using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "Game/Spells/Factory")]
    sealed public class SpellFactory : GameUtils.ScriptableFactory<SpellDefinition> { }
}