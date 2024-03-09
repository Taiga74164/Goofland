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
        [SerializeField] private ParticleSystem effect;
        
        public void Break()
        {
            var position = transform.position;
            AudioManager.Instance.PlayOneShotAudio(breakAudioData, position);
            Instantiate(effect, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
