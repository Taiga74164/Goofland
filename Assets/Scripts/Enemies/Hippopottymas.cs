﻿using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public class Hippopottymas: Enemy
    {
        [Header("Hippopottymas Settings")]
        [SerializeField] private float chargeSpeed = 10.0f;
        [SerializeField] private GameObject disturbedModel;
        public GameEvent onDisturbedEvent;
        
        private bool _isDisturbed;
        private bool _canCharge;
        
        protected override void Update()
        {
            if (GameManager.IsPaused || entityType is not EntityType.Enemy) return;

            if (PlayerInLineOfSight() && !_isDisturbed)
            {
                onDisturbedEvent.Raise(gameObject);
                _isDisturbed = true;
            }

            if (!_isDisturbed) return;
            UpdateChargeCondition();
            Charge();
        }
        
        private void UpdateChargeCondition()
        {
            // Calculate direction to player.
            var playerPosition = playerTransform.position;
            var directionToPlayer = (playerPosition - transform.position).normalized;
    
            // Check if the player is to the left or right of the enemy.
            var isPlayerLeft = directionToPlayer.x < 0;
            var checkDirection = isPlayerLeft ? Vector2.left : Vector2.right;

            // Perform ground and wall checks in the direction towards the player.
            var groundPosition = groundDetection.position;
            var groundForward = groundPosition.Add(checkDirection * rayLength);
            var down = new Vector2(0, -0.5f);
            var groundInfo = Physics2D.Raycast(groundForward, down, rayLength, groundLayer);
            var wallInfo = Physics2D.Raycast(groundPosition, checkDirection, rayLength, turnLayer);

            Debug.DrawLine(groundForward, groundForward.Add(down * rayLength), Color.yellow);
            Debug.DrawLine(groundPosition, groundPosition.Add(checkDirection * rayLength), Color.red);
    
            // Update charge condition based on ground and wall checks.
            _canCharge = groundInfo.collider && !wallInfo.collider;
        }
        
        public void OnDisturbed(object data)
        {
            if (data is not GameObject obj) return;
            if (obj != gameObject) return;
            
            // Destroy the current model and replace it with the disturbed model.
            Destroy(model);
            model = disturbedModel;
            disturbedModel.SetActive(true);
        }

        private void Charge()
        {
            if (!_canCharge) return;
            
            // Get player position.
            var playerPosition = playerTransform.position;
            // Get direction towards player.
            var directionToPlayer = (playerPosition - transform.position).normalized;
            directionToPlayer.y = 0;
            
            // Move towards the player.
            transform.Translate(directionToPlayer * (chargeSpeed * Time.deltaTime));
            // Flip the model based on the direction to the player.
            Flip(directionToPlayer.x);
        }

        private void Flip(float directionX)
        {
            model!.transform.eulerAngles = directionX switch
            {
                < 0 => new Vector3(0, 180, 0),
                > 0 => Vector3.zero,
                _ => model!.transform.eulerAngles
            };
        }
        
        protected override void Patrol()
        {
        }
        
        public override void GotHit(IWeapon weapon)
        {
            if (entityType is not EntityType.Enemy) return;
            
            switch (weapon)
            {
                case Pie when !_isDisturbed:
                    onDisturbedEvent.Raise(gameObject);
                    _isDisturbed = true;
                    break;
                case Piano:
                    OnHit();
                    break;
            }
        }
    }
}