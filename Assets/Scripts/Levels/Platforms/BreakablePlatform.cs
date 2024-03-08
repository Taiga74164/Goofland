using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class BreakablePlatform : MonoBehaviour, IBreakable
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioData breakAudioData;
        
        public void Break()
        {
            AudioManager.Instance.PlayOneShotAudio(breakAudioData, transform.position);
            Destroy(gameObject);
        }
    }
}
