using Controller;
using UnityEngine;

namespace Levels
{
    public class Pitfall : MonoBehaviour
    {
        public Transform respawnPoint;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.IsPlayer()) return;
            
            other.GetComponent<PlayerController>().TakeDamage();
            other.transform.position = respawnPoint.position;
        }
    }
}
