using Controllers;
using UnityEngine;

namespace Enemies.Components
{
    public class MusicNoteProjectile : EnemyBase
    {
        public Vector2 direction = Vector2.left;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float maxDistance = 5.0f;
        
        private Rigidbody2D _rb;
        private Vector2 _startPosition;

        private void Start()
        {
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
            
            // Set the rigidbody to kinematic.
            _rb.isKinematic = true;
            
            // Set the start position.
            _startPosition = transform.position;
            
            // Set the velocity of the projectile.
            _rb.velocity = direction * speed;
        }

        private void Update()
        {
            // If the projectile has traveled the maximum distance, destroy it.
            if (Vector2.Distance(_startPosition, transform.position) >= maxDistance)
                Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<PlayerController>()) return;
            
            other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy: this);
            Destroy(gameObject);
        }
    }
}