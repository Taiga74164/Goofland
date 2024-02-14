using Enemies.Components;
using Managers;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public class DurumaDiver : Enemy
    {
        [Header("DurumaDiver Settings")] 
        [Tooltip("The delay between attacks.")]
        [SerializeField] private float attackDelay = 3.0f;
        [Tooltip("The range at which the DurumaDiver will attack the player.")]
        [SerializeField] private float attackRange = 2.0f;
        [Tooltip("The speed at which the DurumaDiver will dive bomb the player.")]
        [SerializeField] private float diveSpeed = 5.0f;

        private DurumaHead _headComponent;
        private float _attackTimer;

        protected override void Start()
        {
            base.Start();
            
            // TODO: Change this when Asset and Animation is ready.
            // Get the DurumaHead component from the model or the children.
            _headComponent = model!.GetComponent<DurumaHead>() ? 
                model.GetComponent<DurumaHead>() : GetComponentInChildren<DurumaHead>();
        }

        protected override void Update()
        {
            if (GameManager.IsPaused) return;

            HandleAttack();
        }

        private void HandleAttack()
        {
            if (Time.time >= _attackTimer + attackDelay && PlayerInLineOfSight() && InAttackRange())
            {
                Attack();
                _attackTimer = Time.time;
            }
        }

        private bool InAttackRange()
        {
            var playerPosition = GameManager.Instance.playerController.transform.position;
            return Vector2.Distance(playerPosition, transform.position) <= attackRange;
        }

        private void Attack()
        {
            var playerPosition = GameManager.Instance.playerController.transform.position;
            var directionToPlayer = (playerPosition - transform.position).normalized;
            rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * diveSpeed;
        }
        
        public override void GotHit(IWeapon weapon)
        {
            if (weapon is Pie && _headComponent.stacks <= 0)
                Die();
        }
    }
}
