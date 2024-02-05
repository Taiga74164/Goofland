using UnityEngine;
using UnityEngine.Audio;

namespace Objects.Scriptable
{
    [CreateAssetMenu(fileName = "AudioData")]
    public class AudioData : ScriptableObject
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public bool playOnAwake;
        public bool loop;
    }
}