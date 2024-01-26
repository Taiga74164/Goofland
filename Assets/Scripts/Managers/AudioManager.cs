using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public void PlayAudio(Component sender, object data)
        {
            if (data is AudioSource audioSource)
            {
                audioSource.Play();
            }
        }

        public void StopAudio(Component sender, object data)
        {
            if (data is AudioSource audioSource)
            {
                audioSource.Stop();
            }
        }
    }
}

