using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public void PlayAudio(AudioSource sound)
        {
            sound.Play();
        }

        public void StopAudio(AudioSource sound)
        {
            sound.Stop();
        }
    }
}

