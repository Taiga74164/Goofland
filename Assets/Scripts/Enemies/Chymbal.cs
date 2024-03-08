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
        [SerializeField] private GameObject noteSpawnPoint;
        
        private float _attackCooldown;
        
        protected override void Start()
        {
            base.Start();
            
            // Set the next attack time.
            _attackCooldown = attackInterval;
        }

        protected override void Update()
        {
            base.Update();
            
            if (GameManager.IsPaused) return;
            
            HandleAttack();
        }
        
        private void HandleAttack()
        {
            if (entityType is not EntityType.Enemy) return;
            
            // Decrement the next attack time.
            _attackCooldown -= Time.deltaTime;
            
            if (_attackCooldown <= 0 && PlayerInLineOfSight())
            {
                // Turn towards the player and attack.
                Turn();
                Attack();
                
                // Set the next attack time.
                _attackCooldown = attackInterval;
            }
        }

        private void Attack()
        {
            // Create a music note projectile.
            var projectile = PrefabManager.Create<MusicNoteProjectile>(Prefabs.MusicNoteProjectile, transform);
            // Set the projectile's position.
            projectile.transform.position = noteSpawnPoint.transform.position;
            // Set the projectile's direction.
            projectile.direction = direction;
        }
        
        protected override void Patrol()
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
    }
}