using UnityEngine;

namespace Enemies.Components
{
    public class DurumaStack : Enemy
    {
        protected override void Start()
        {
            base.Start();
            
            AdjustGroundDetectionPosition();
        }

        private void AdjustGroundDetectionPosition()
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            var colliderBottom = boxCollider.offset.y - (boxCollider.size.y / 2);
            groundDetection.localPosition = new Vector2(groundDetection.localPosition.x, colliderBottom);
        }
        
        protected override void Update() { }

        protected override void FixedUpdate() { }

        protected override void Patrol() { }

        protected override void Turn() { }

        protected internal override void OnHit()
        {
            // Play particle effect.
            
            // Drop dice when the enemy is hit.
            DropDice();
            
            // Destroy the enemy.
            Destroy(gameObject);
        }

        public override void GotHit(IWeapon weapon) { }
    }
}