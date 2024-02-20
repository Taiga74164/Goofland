using Managers;
using UnityEngine;

namespace Enemies
{
    public class RubbaDuckie : Enemy
    {
        [Header("RubbaDuckie Settings")]
        [Tooltip("The maximum height the duck will bounce.")]
        [SerializeField] private float bounceHeight = 0.5f;
        [Tooltip("The speed at which the duck will bounce.")]
        [SerializeField] private float bounceSpeed = 10.0f;
        
        private float _startY;
        private float _bounceTimer;
        
        protected override void Start()
        {
            _startY = transform.position.y;
        }

        protected override void Update()
        {
            if (GameManager.IsPaused) return;
            
            Bounce();
        }
        
        protected override void Patrol()
        {
            // Move the enemy in the direction it is facing.
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

            var groundPosition = groundDetection.position;
            var wallInfo = Physics2D.Raycast(groundPosition, direction, rayLength, turnLayer);

            if (wallInfo.collider)
                Turn();
        }
        
        private void Bounce()
        {
            // Update the bounce timer.
            _bounceTimer += Time.deltaTime * bounceSpeed;
            // Calculate the new Y position of the duck.
            var newY = _startY + Mathf.Sin(_bounceTimer) * bounceHeight;
            
            // Update the position of the duck.
            transform.position = new Vector2(transform.position.x, newY);
        }
    }
}