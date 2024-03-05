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
        private bool _isGrounded;

        protected override void Update()
        {
            base.Update();
            
            if (!_isGrounded)
                CheckForGround();
            else
                Bounce();
        }
        
        protected override void Patrol()
        {
            // Move the enemy in the direction it is facing.
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
            
            var wallInfo = Physics2D.Raycast(transform.position, direction, rayLength, turnLayer);
            
            if (wallInfo.collider)
                Turn();
        }

        private void CheckForGround()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, 10.0f, groundLayer);
            _startY = hit.point.y;
            _isGrounded = hit.collider != null;
            _bounceTimer = Mathf.PI / 2;
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