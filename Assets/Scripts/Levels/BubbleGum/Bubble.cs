using Controllers;
using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private float explosionForce;
        [SerializeField] private AudioData popAudioData;
        
        private void Start() => gameObject.SetActive(false);

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.IsPlayer()) return;
            AudioManager.Instance.PlayOneShotAudio(popAudioData, transform.position);
            var force = -collision.relativeVelocity.normalized * explosionForce;
            collision.gameObject.GetComponent<PlayerController>().Bounce(force);
            TimerManager.Instance.StartTimer(popAudioData.clip.length, () => gameObject.SetActive(false));
        }
    }
}

