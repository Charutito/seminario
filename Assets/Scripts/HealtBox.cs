﻿using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;

public class HealtBox : Collectable
{
    public int HealingPow;

    protected override void Collect()
    {
        GameManager.Instance.Character.Heal(HealingPow);
    }
}
