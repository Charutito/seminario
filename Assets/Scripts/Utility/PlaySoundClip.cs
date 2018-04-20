using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundClip : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [SerializeField] private bool testSound;

    public void PlayClip()
    {
        if (source != null)
        {
            source.Play();
        }
    }

    private void Update()
    {
        if (testSound)
        {
            testSound = false;
            PlayClip();
        }
    }
}
