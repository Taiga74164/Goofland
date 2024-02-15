using System;
using Objects.Scriptable;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Managers
{
    /// <summary>
    /// Manages audio globally for the game.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer audioMixer;
        private AudioSource _audioSource;

        /// <summary>
        /// Special singleton initializer method.
        /// </summary>
        public new static void Initialize()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Managers/[AudioManager]");
            if (prefab == null) throw new Exception("Missing AudioManager prefab!");

            var instance = Instantiate(prefab);
            if (instance == null) throw new Exception("Failed to instantiate AudioManager prefab!");

            instance.name = "Managers.AudioManager (Singleton)";
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _audioSource = GetComponent<AudioSource>();
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
            SetBGMVolume(PlayerPrefsUtil.BGMVolume);
            SetSFXVolume(PlayerPrefsUtil.SFXVolume);
            SetMasterVolume(PlayerPrefsUtil.MasterVolume);
        }

        public void SetMasterVolume(float value) => 
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        
        public void SetBGMVolume(float value) => 
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        
        public void SetSFXVolume(float value) => 
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}

