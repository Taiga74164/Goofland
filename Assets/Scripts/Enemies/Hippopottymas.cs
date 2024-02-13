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
        private Rigidbody2D _rb;

        protected override void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        protected override void Update()
        {
            if (GameManager.IsPaused) return;

            if (PlayerInLineOfSight())
                _isDisturbed = true;
            
            Charge();
        }

        private void Charge()
        {
            if (!_isDisturbed) return;
            
            // Get player.
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Get direction towards player.
            var directionToPlayer = (playerPosition - transform.position).normalized;
            
            // Move towards the player.
            _rb.velocity = new Vector2(directionToPlayer.x * chargeSpeed, _rb.velocity.y);
        }
        
        protected override void MoveEnemy()
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
                    Die();
                    break;
            }
        }
    }
}