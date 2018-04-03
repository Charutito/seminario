using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public enum GroupAction
    {
        None = 1,
        OutOfControl = 2,

        // Agressive
        Attacking = 10,
        SpecialAttack = 11,

        // Movement
        Stalking = 20,
        Following = 21
    }
}
