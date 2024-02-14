using Controller;
using UnityEngine;

namespace Enemies.Components
{
    public class DurumaStack : Enemy
    {
        protected override void Awake() { }

        protected override void Start() { }

        protected override void Update() { }

        protected override void FixedUpdate() { }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer())
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy: transform);
        }

        protected override void MoveEnemy() { }

        protected override void Timer() { }

        protected override void Turn() { }

        public override void GotHit(IWeapon weapon) { }


    }
}