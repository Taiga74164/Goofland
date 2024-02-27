using Managers;
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
        
        protected override void Update()
        {
            if (GameManager.IsPaused || entityType is not EntityType.Enemy) return;

            if (PlayerInLineOfSight() && !_isDisturbed)
            {
                onDisturbedEvent.Raise(gameObject);
                _isDisturbed = true;
            }
            
            Charge();
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
            if (!_isDisturbed) return;
            
            // Get player position.
            var playerPosition = playerTransform.position;
            // Get direction towards player.
            var directionToPlayer = (playerPosition - transform.position).normalized;
            
            // Move towards the player.
            rb.velocity = new Vector2(directionToPlayer.x * chargeSpeed, rb.velocity.y);
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
        
        protected override void Turn()
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