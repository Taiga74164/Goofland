using System.Collections.Generic;
using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Levels
{
    public class Balloon : MonoBehaviour
    {
        [Header("Balloon Settings")]
        [SerializeField] private bool respawning;
        
        [Header("Piano Settings")]
        [SerializeField] private Piano piano;
        [SerializeField] private bool despawn;
        
        [Header("Audio Settings")]
        [SerializeField] private List<AudioData> balloonAudioDatas;

        [Header("Particle Effect")]
        public ParticleSystem effect;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<Pie>()) return;
            
            // Detach the piano from the balloon and drop it.
            piano.transform.SetParent(null);
            piano.despawn = despawn;
            piano.DropPiano();
            
            // Play a random audio clip from the list.
            var randomIndex = Random.Range(0, balloonAudioDatas.Count - 1);
            var position = transform.position;
            AudioManager.Instance.PlayOneShotAudio(balloonAudioDatas[randomIndex], position);

            Instantiate(effect, position, Quaternion.identity);

            if (!respawning)
            {
                Destroy(gameObject, balloonAudioDatas[randomIndex].clip.length);
            }
            else
            {
                piano.transform.SetParent(gameObject.transform);
                GetComponent<SpriteRenderer>().enabled = GetComponent<CircleCollider2D>().enabled = false;
            }
        }

        public void Respawn() => 
            GetComponent<SpriteRenderer>().enabled = GetComponent<CircleCollider2D>().enabled = true;
    }
}