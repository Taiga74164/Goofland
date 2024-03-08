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
            var prefab = Resources.Load<GameObject>("Prefabs/Managers/AudioManager");
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

        /// <summary>
        /// Plays a one-shot audio clip with the given AudioData.
        /// </summary>
        /// <param name="data">The AudioData to play.</param>
        /// <param name="position">The position to play the audio at.</param>
        /// <param name="proximity">Play the audio based on proximity to the player.</param>
        /// <param name="maxProximityDistance">The maximum distance to play the audio at.</param>
        /// <exception cref="Exception">If the AudioData is null or missing a clip.</exception>
        public void PlayOneShotAudio(AudioData data, Vector3 position, bool proximity = true, float maxProximityDistance = 10.0f)
        {
            if (!data || !data.clip) throw new Exception("AudioData is null or missing a clip!");
            
            // Create a temporary audio source to play the audio.
            var tempObject = new GameObject("TempAudio") { transform = { position = position } };

            // Add an audio source to the temporary object.
            var tempAudioSource = tempObject.AddComponent<AudioSource>();
            
            // Configure the audio source with the given AudioData.
            tempAudioSource.Configure(data);
            
            // If the audio should play based on proximity to the player, set the volume based on distance.
            if (proximity)
            {
                var distance = Vector3.Distance(position, GameManager.Instance.playerController.transform.position);
                var volume = Mathf.Clamp01(1 - distance / maxProximityDistance);
                tempAudioSource.volume = volume;
            }
            
            // Play the audio source.
            tempAudioSource.Play();
            
            // Destroy the temporary object after the audio clip has finished playing.
            Destroy(tempObject, data.clip.length);
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

