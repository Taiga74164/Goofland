using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _mainTheme;
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
    }
}

