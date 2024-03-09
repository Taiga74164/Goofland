using Controllers;
using UnityEngine;

namespace Enemies.Components
{
    public class FartCloudEffect : EnemyBase
    {
        [Header("Fart Cloud Settings")]
        public float fartArea;
        private CircleCollider2D _circleCollider2D;
        
        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem effect;

        private void Start()
        {
            // Get the circle collider 2D component.
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            // Set the radius of the circle collider 2D.
            _circleCollider2D.radius = fartArea;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.IsPlayer()) return; 
            var player = other.GetComponent<PlayerController>();
            player.TakeDamage(enemy: this);
            Instantiate(effect, player.transform.position, Quaternion.identity);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fartArea);
        }
#endif
    }
}