using Controller;
using Enemies.Projectile;
using Managers;
using Objects.Scriptable;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Chymbal : Enemy
    {
        [Header("Attack Settings")]
        [SerializeField] private float attackInterval = 3.0f;
        [SerializeField] private float lineOfSight = 10.0f;
    
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
            HandleProximity();
        }
    
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (GameManager.IsPaused) return;
        
            HandleAttack();
        }
        
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.IsPlayer()) return;
            
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage();
            player.KnockBack(transform);
        }
    
        private void HandleAttack()
        {
            if (Time.time >= _nextAttackTime && PlayerInLineOfSight())
            {
                Turn();
                Attack();
                _nextAttackTime = Time.time + attackInterval;
            }
        }
        
        private bool PlayerInLineOfSight()
        {
            var playerTransform = GameManager.Instance.playerController.transform;
            var direction = playerTransform.position - transform.position;
            var hit = Physics2D.Raycast(transform.position, 
                direction, lineOfSight, 
                ~LayerMask.NameToLayer("Player"));
            return hit.collider != null && hit.collider.CompareTag("Player");
        }

        protected override void Turn()
        {
            var playerPosition = GameManager.Instance.playerController.transform.position;
            var playerIsRight = playerPosition.x > transform.position.x;
            
            direction = playerIsRight ? Vector2.right : Vector2.left;
            model!.transform.eulerAngles = playerIsRight ? Vector3.zero : new Vector3(0, 180, 0);
        }

        private void Attack()
        {
            var projectile = PrefabManager.Create<MusicNoteProjectile>(Prefabs.MusicNoteProjectile, transform);
            projectile.direction = direction;
        }

        protected override void MoveEnemy()
        {
        }

        private void HandleProximity()
        {
            var playerTransform = GameManager.Instance.playerController.transform;
            var distance = Vector3.Distance(transform.position, playerTransform.position);
            var volume = Mathf.Clamp01(1 - distance / maxProximityDistance);
            _audioSource.volume = volume;
        }
    
        protected override void Die()
        {
            _audioSource.Configure(deathAudioData);
            _audioSource.Play();
            base.Die();
        }
    }
}