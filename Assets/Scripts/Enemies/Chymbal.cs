using Controllers;
using Enemies.Components;
using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Enemies
{
    public class Chymbal : Enemy
    {
        [Header("Attack Settings")]
        [Tooltip("The delay between attacks.")]
        [SerializeField] private float attackInterval = 3.0f;
    
        [Header("Audio Settings")]
        [SerializeField] private AudioData audioData;
        [SerializeField] private AudioData deathAudioData;
        [SerializeField] private float maxProximityDistance = 10.0f;
        
        private float _nextAttackTime;
        private AudioSource _audioSource;

        protected override void Start()
        {
            // Get the audio source component and configure it.
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Configure(audioData);
            _audioSource.Play();
            _audioSource.volume = 0.0f;
        }

        protected override void Update()
        {
            if (GameManager.IsPaused) return;

            HandleProximity();
            HandleAttack();
        }
        
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.IsPlayer()) return;
            
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage, transform);
        }
        
        private void HandleAttack()
        {
            if (Time.time >= _nextAttackTime && PlayerInLineOfSight())
            {
                // Turn towards the player and attack.
                Turn();
                Attack();
                
                // Set the next attack time.
                _nextAttackTime = Time.time + attackInterval;
            }
        }

        private void Attack()
        {
            // Create a music note projectile.
            var projectile = PrefabManager.Create<MusicNoteProjectile>(Prefabs.MusicNoteProjectile, transform);
            // Set the projectile's direction.
            projectile.direction = direction;
        }
        
        protected override void MoveEnemy()
        {
        }
        
        protected override void Turn()
        {
            // Get the player's position.
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Determine the direction to face.
            var playerIsRight = playerPosition.x > transform.position.x;
            
            // Set the direction and model rotation.
            direction = playerIsRight ? Vector2.right : Vector2.left;
            model!.transform.eulerAngles = playerIsRight ? Vector3.zero : new Vector3(0, 180, 0);
        }
        
        private void HandleProximity()
        {
            var playerTransform = GameManager.Instance.playerController.transform;
            var distance = Vector3.Distance(transform.position, playerTransform.position);
            var volume = Mathf.Clamp01(1 - distance / maxProximityDistance);
            _audioSource.volume = volume;
        }
    
        protected internal override void Die()
        {
            _audioSource.Configure(deathAudioData);
            _audioSource.Play();
            base.Die();
        }
    }
}