using UnityEngine;
using UnityEngine.Audio;

namespace Objects.Scriptable
{
    /// <summary>
    /// Represents audio data for use in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioData")]
    public class AudioData : ScriptableObject
    {
        [Tooltip("The audio clip to play.")]
        public AudioClip clip;
        
        [Tooltip("The audio mixer group the clip is routed through.")]
        public AudioMixerGroup mixerGroup;
        
        [Tooltip("Should the audio play as soon as the game starts.")]
        public bool playOnAwake;
        
        [Tooltip("Should the audio loop.")]
        public bool loop;
        
        [Tooltip("The volume of the audio.")]
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
    }
}