using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer audioMixer;

        protected override void Awake()
        {
            base.Awake();
            SetBGMVolume(PlayerPrefsManager.BGMVolume);
            SetSFXVolume(PlayerPrefsManager.SFXVolume);
            SetMasterVolume(PlayerPrefsManager.MasterVolume);
        }
        
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

        public void SetMasterVolume(float value)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        }
        
        public void SetBGMVolume(float value)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }
        
        public void SetSFXVolume(float value)
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        }
    }
}

