using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundClip : MonoBehaviour
{
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        
        Destroy(gameObject, _source.clip.length);
    }
}
