namespace Enemies.Components
{
    public class DurumaStack : Enemy
    {
        protected override void Awake() { }

        protected override void Start() { }

        protected override void Update() { }

        protected override void FixedUpdate() { }

        protected override void Patrol() { }

        protected override void Turn() { }

        protected internal override void OnHit()
        {
            // Play particle effect.
            
            // Destroy the enemy.
            Destroy(gameObject);
        }

        public override void GotHit(IWeapon weapon) { }
    }
}