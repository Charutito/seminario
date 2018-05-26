using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace Entities
{
    public class EntitySounds : MonoBehaviour
    {
        [Serializable]
        public class EntitySoundDictionary
        {
            public AudioEvent Clip;
            public string EffectName;
        }

        public List<EntitySoundDictionary> Effects;

        public void PlayEffect(string effectName)
        {
            var audioEvent = Effects.FirstOrDefault(x => x.EffectName == effectName);

            if (audioEvent != null)
            {
                audioEvent.Clip.Play();
            }
        }
        
        public void PlayEffect(string effectName, Vector3 position)
        {
            var audioEvent = Effects.FirstOrDefault(x => x.EffectName == effectName);

            if (audioEvent != null)
            {
                audioEvent.Clip.Play(position);
            }
        }
    }
}