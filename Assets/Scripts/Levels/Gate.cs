using System;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class Gate : MonoBehaviour, ITrigger
    {
        public AudioData audioData;
        private AudioSource _audioSource;

        public GameEvent onTrigger;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Configure(audioData);
        }
        
        public void Trigger()
        {
            onTrigger.Raise(_audioSource);
            // TODO: Change to animation.
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}