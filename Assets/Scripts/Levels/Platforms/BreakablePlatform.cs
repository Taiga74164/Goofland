using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class BreakablePlatform : MonoBehaviour, IBreakable
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioData breakAudioData;

        [Header("Particle Effects")]
        public ParticleSystem effect;
        public void Break()
        {
            AudioManager.Instance.PlayOneShotAudio(breakAudioData, transform.position);
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
