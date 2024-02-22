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
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<Pie>()) return;
            
            // Detach the piano from the balloon and drop it
            piano.transform.SetParent(null);
            piano.despawn = despawn;
            piano.DropPiano();

            if (!respawning)
            {
                Destroy(gameObject);
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