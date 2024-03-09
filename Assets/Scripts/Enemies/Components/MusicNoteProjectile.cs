using Controllers;
using UnityEngine;

namespace Enemies.Components
{
    public class MusicNoteProjectile : EnemyBase
    {
        [Header("Music Note Settings")]
        public Vector2 direction = Vector2.left;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float maxDistance = 5.0f;
        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;
        
        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem effect;
        
        private Rigidbody2D _rb;
        private Vector2 _startPosition;
        private SpriteRenderer _spriteRenderer;
        private Chymbal _chymbal;
        
        private void Start()
        {
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
            
            // Set the rigidbody to kinematic.
            _rb.isKinematic = true;
            
            // Set the start position.
            _startPosition = transform.position;
            
            // Get the sprite renderer component.
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Set the sprite based on the direction.
            _spriteRenderer.sprite = direction.x < 0 ? leftSprite : rightSprite;
            
            // Get the chymbal component.
            _chymbal = GetComponentInParent<Chymbal>();
            
            // Set the velocity of the projectile.
            _rb.velocity = direction * speed;
        }

        private void Update()
        {
            // If the projectile has traveled the maximum distance, destroy it.
            if (Vector2.Distance(_startPosition, transform.position) >= maxDistance)
                Destroy(gameObject);
            
            if (_chymbal.entityType is not EntityType.Enemy) Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.GetComponent<PlayerController>()) return;
            
            var player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(enemy: this);
            Instantiate(effect, player.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}