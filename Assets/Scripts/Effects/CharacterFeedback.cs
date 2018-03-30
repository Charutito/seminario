using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

public class CharacterFeedback : MonoBehaviour
{
    public GameObject rightSlash;
    public GameObject leftSlash;
    public GameObject heavySlash;
    public GameObject hitPart;
    public Transform Slash1Pos;
    public Transform Slash2Pos;
    public Transform SlashHeavyPos;

    public void RightSlashEffect()
    {
        var part = Instantiate(rightSlash, Slash1Pos.position, Slash1Pos.rotation, Slash1Pos);
        Destroy(part, 1);
    }

    public void LeftSlashEffect()
    {
        var part = Instantiate(leftSlash, Slash2Pos.position, Slash2Pos.rotation, Slash2Pos);
        Destroy(part, 1);
    }

    public void HeavySlashEffect()
    {
        var part = Instantiate(heavySlash, SlashHeavyPos.position, SlashHeavyPos.rotation, SlashHeavyPos);
        Destroy(part, 1);
    }
}
