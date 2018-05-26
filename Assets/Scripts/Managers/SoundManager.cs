using System.Collections;
using System.Collections.Generic;
using GameUtils;
using Managers;
using UnityEngine;

namespace Managers
{
    public class SoundManager : SingletonObject<SoundManager>
    {
        public GameObject PointSoundPrefab;
            
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

        public void PlayAtPoint(AudioEvent evt, Vector3 position)
        {
            if (evt == null) return;

            var newSound = Instantiate(PointSoundPrefab, position, Quaternion.identity);

            var audioSource = newSound.GetComponent<AudioSource>();
            
            evt.Play(audioSource);
        }
    }
}
