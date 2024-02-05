using Objects.Scriptable;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer audioMixer;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            
            _audioSource = GetComponent<AudioSource>();
            
            // Load volume settings from PlayerPrefs.
            LoadVolumeSettings();
        }
        
        public void PlayAudio(object data)
        {
            if (data is AudioSource audioSource)
                audioSource.Play();
        }

        public void PlayAudio(AudioData data)
        {
            _audioSource.Configure(data);
            _audioSource.Play();
        }

        public void StopAudio(object data)
        {
            if (data is AudioSource audioSource)
                audioSource.Stop();
        }
        
        private void LoadVolumeSettings()
        {
            SetBGMVolume(PlayerPrefsManager.BGMVolume);
            SetSFXVolume(PlayerPrefsManager.SFXVolume);
            SetMasterVolume(PlayerPrefsManager.MasterVolume);
        }

        public void SetMasterVolume(float value) => 
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        
        public void SetBGMVolume(float value) => 
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        
        public void SetSFXVolume(float value) => 
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}

