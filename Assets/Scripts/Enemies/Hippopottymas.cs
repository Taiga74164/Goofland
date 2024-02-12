using Managers;
using UnityEngine;

namespace Enemies
{
    public class Hippopottymas: Enemy
    {
        /*
         * Known as the most aggressive enemy to mankind, this hippo enjoys sitting on a stationary toilet and singing.
         * This enemy is invincible to all your attacks and is extremely aggressive when disturbed.
         * The hippo will charge at the player mercilessly and instantly murder the player.
         * They can only be defeated by a falling piano
         */
        [Header("Hippopottymas Settings")]
        [SerializeField] private float chargeSpeed = 10.0f;

        public bool Distrubed { get; set; }

        protected override void Update()
        {
            if (GameManager.IsPaused) return;

            // Debugging
            if (Input.GetKey(KeyCode.CapsLock))
                Distrubed = true;
            
            if (Distrubed)
                Charge();
        }

        private void Charge()
        {
            // TODO: 
            // 1. Add ground check.
            
            // Get player.
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Get direction towards player.
            var directionToPlayer = (playerPosition - transform.position).normalized;
            
            // Move towards the player.
            transform.position += directionToPlayer * (chargeSpeed * Time.deltaTime);
        }
        
        protected override void MoveEnemy()
        {
        }
        
        protected override void Turn()
        {
        }
        
        public override void GotHit(IWeapon weapon)
        {
        }
        
    }
}