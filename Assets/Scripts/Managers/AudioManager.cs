using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
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

