using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer audioMixer;
        
        public void PlayAudio(object data)
        {
            if (data is AudioSource audioSource)
            {
                audioSource.Play();
            }
        }

        public void StopAudio(object data)
        {
            if (data is AudioSource audioSource)
            {
                audioSource.Stop();
            }
        }

        public void SetBGMVolume(float value)
        {
            audioMixer.SetFloat("BGMVolume", value);
        }
        
        public void SetSFXVolume(float value)
        {
            audioMixer.SetFloat("SFXVolume", value);
        }
    }
}

