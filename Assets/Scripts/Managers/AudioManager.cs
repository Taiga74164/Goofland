using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public void PlayAudio(Component sender, object data)
        {
            
            if(data is AudioSource)
            {
                AudioSource audioSource = (AudioSource) data;
                audioSource?.Play();
            }
                
            
            
        }

        public void StopAudio(Component sender, object data)
        {
            if (data is AudioSource)
            {
                AudioSource audioSource = (AudioSource)data;
                audioSource?.Stop();
            }
        }
    }
}

