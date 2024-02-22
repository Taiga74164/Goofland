using Managers;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public class Hippopottymas: Enemy
    {
        [Header("Hippopottymas Settings")]
        [SerializeField] private float chargeSpeed = 10.0f;

        private bool _isDisturbed;
        
        protected override void Update()
        {
            if (GameManager.IsPaused) return;

            if (PlayerInLineOfSight())
                _isDisturbed = true;
            
            Charge();
        }

        private void Charge()
        {
            if (!_isDisturbed || entityType is not EntityType.Enemy) return;
            
            // Get player.
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Get direction towards player.
            var directionToPlayer = (playerPosition - transform.position).normalized;
            
            // Move towards the player.
            rb.velocity = new Vector2(directionToPlayer.x * chargeSpeed, rb.velocity.y);
        }
        
        protected override void Patrol()
        {
        }
        
        protected override void Turn()
        {
        }
        
        public override void GotHit(IWeapon weapon)
        {
            switch (weapon)
            {
                case Pie:
                    _isDisturbed = true;
                    break;
                case Piano:
                    OnHit();
                    break;
            }
        }
    }
}