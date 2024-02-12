using Enemies.Components;
using Managers;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public class DurumaDiver : Enemy
    {
        /*
         * Has to be hit on the head to be defeated.
         * You can remove the stacked items below by hitting them with the pie.
         * They essentially function like stacking enemies except that the real enemy is on the top and avoids damage by shedding the stacks they stand on.
         * The enemy attacks by divebombing on you within 2 blocks. There is a delay of 3 seconds.
         * Moves in a straight line and turns around at ledges and walls.
         */ 
        [Header("DurumaDiver Settings")] 
        [Tooltip("The delay between attacks.")]
        [SerializeField] private float attackDelay = 3.0f;
        [Tooltip("The range at which the DurumaDiver will attack the player.")]
        [SerializeField] private float attackRange = 2.0f;
        [Tooltip("The speed at which the DurumaDiver will dive bomb the player.")]
        [SerializeField] private float diveSpeed = 5.0f;

        private DurumaDiverHead _headComponent;
        private Rigidbody2D _rb;
        private float _attackTimer;

        protected override void Start()
        {
            base.Start();
            
            // Get the DurumaDiverHead component from the model or the children.
            _headComponent = model!.GetComponent<DurumaDiverHead>() ? 
                model.GetComponent<DurumaDiverHead>() : GetComponentInChildren<DurumaDiverHead>();
            
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
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
            _rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * diveSpeed;
        }
        
        public override void GotHit(IWeapon weapon)
        {
            if (weapon is Pie && _headComponent.stacks <= 0)
                Die();
        }
    }
}
