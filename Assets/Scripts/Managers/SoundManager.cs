using System.Collections;
using System.Collections.Generic;
using GameUtils;
using Managers;
using UnityEngine;

namespace Managers
{
    public class SoundManager : SingletonObject<SoundManager>
    {
        private AudioSource[] _sources;

        private void Awake()
        {
            _sources = GetComponents<AudioSource>();
        }

        public void Play(AudioEvent evt)
        {
            if (evt == null) return;
            
            foreach (var source in _sources)
            {
                if(!source.isPlaying)
                {
                    evt.Play(source);
                    break;
                }
            }
        }
    }
}
