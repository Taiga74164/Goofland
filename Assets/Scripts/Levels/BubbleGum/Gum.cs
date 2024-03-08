using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class Gum : MonoBehaviour
    {
        [SerializeField] private AudioData inflateAudioData;
        [SerializeField] private AudioData gumBallMachineAudioData;
        
        private Bubble _bubble;
        
        private void Awake() => _bubble = GetComponentInChildren<Bubble>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareLayer("Projectile"))
            {
                var position = transform.position;
                // Play the inflate sound.
                AudioManager.Instance.PlayOneShotAudio(inflateAudioData, position);
                // After the inflate sound, play the gum ball machine sound.
                TimerManager.Instance.StartTimer(inflateAudioData.clip.length, () =>
                    AudioManager.Instance.PlayOneShotAudio(gumBallMachineAudioData, position));
                
                _bubble.gameObject.SetActive(true);
            }
        }
    }
}
