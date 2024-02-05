using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class Gate : MonoBehaviour, ITrigger
    {
        public AudioData audioData;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Configure(audioData);
        }
        
        public void Trigger()
        {
            // Temporary.
            gameObject.SetActive(!gameObject.activeSelf);
            _audioSource.Play();
        }
    }
}