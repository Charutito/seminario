using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class CharacterFeedback : MonoBehaviour {
    public GameObject slashLight1;
    public GameObject slashLight2;
    public GameObject slashHeavy;
    public GameObject hitPart;
    public CharacterEntity Char;
    public Transform Slash1Pos;
    public Transform Slash2Pos;
    public Transform SlashHeavyPos;

    public void Awake() {
        Char.OnAttack += LightAttack1;
        Char.OnHeavyAttack += heavyAttack;

    }
    public void LightAttack1()
    {
        var atkn = 1;
        if (atkn==1)
        {
            var part = Instantiate<GameObject>(slashLight1, Slash1Pos.position, Slash1Pos.rotation, Slash1Pos);
            Destroy(part, 1);
        }else
        {
            var part = Instantiate<GameObject>(slashLight2, Slash2Pos.position, Slash2Pos.rotation, Slash2Pos);
            Destroy(part, 1);
        }       
    }

    public void heavyAttack()
    {
        var part = Instantiate<GameObject>(slashHeavy, SlashHeavyPos.position, SlashHeavyPos.rotation,SlashHeavyPos);
        Destroy(part, 1);
    }
}
