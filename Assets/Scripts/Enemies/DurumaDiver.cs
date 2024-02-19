using Controllers;
using Enemies.Components;
using JetBrains.Annotations;
using Managers;
using Objects.Scriptable;
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
        [SerializeField] private Transform stackContainer;
        [Tooltip("The number of stacks the DurumaDiver has.")]
        [SerializeField] private int stacks = 2; 

        private float _attackTimer;
        private Vector3 _startGroundDetectionPosition;

        protected override void Start()
        {
            base.Start();
            
            // Set the start ground detection position.
            _startGroundDetectionPosition = groundDetection.position;
            
            // Generate the stacks.
            GenerateStack(stacks);
        }

        protected override void Update()
        {
            if (GameManager.IsPaused) return;
            
            // Update the number of stacks.
            stacks = stackContainer.childCount;
            
            // Handle the DurumaDiver's attack.
            HandleAttack();
        }
        
        protected override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer())
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy: this);
            else if (other.gameObject.GetComponent<Pie>())
                ShedStack();
        }
        
        protected override void Patrol()
        {
            base.Patrol();
            //
            // if (stacks <= 0)
            // {
            //     base.Patrol();
            // }
            // else
            // {
            //     // Move the child in the direction it is facing.
            //     transform.Translate(direction * (speed * Time.deltaTime));
            //     
            //     // Get the last child in the stack container.
            //     var lastChild = stackContainer.GetChild(stacks - 1);
            //     // Get the last child's ground check position.
            //     var groundPosition = lastChild.GetChild(0).position;
            //     var groundInfo = Physics2D.Raycast(groundPosition, Vector2.down, rayLength, 
            //         groundLayer);
            //     var wallInfo = Physics2D.Raycast(groundPosition, direction, rayLength, turnLayer);
            //     
            //     Debug.DrawLine(groundPosition, groundPosition + new Vector3(direction.x, direction.y, 0) * rayLength, Color.red);
            //     
            //     if (!groundInfo.collider)
            //         Turn();
            //     if (!wallInfo.collider)
            //         Turn();
            // }
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
        
        private void GenerateStack(int count)
        {
            float totalHeight = 0;
            for (var i = 0; i < count; i++)
            {
                var stack = PrefabManager.Create(Prefabs.DurumaDiverStack, stackContainer.transform);
                // TODO: Change this when Asset and Animation is ready since it won't have a SpriteRenderer.
                var stackHeight = stack.GetComponent<SpriteRenderer>().bounds.size.y;

                // Subtract the height of the new stack from the total height.
                totalHeight -= stackHeight;

                // Set the position of the stack.
                stack.transform.localPosition = new Vector3(0, totalHeight, 0);
                stack.name = $"Stack {i + 1}";

                // Add the height of the new stack to the total height.
                totalHeight += stackHeight;
            }

            // Set the position of the DurumaHead to the top of the stacks.
            transform.position = new Vector3(transform.position.x, -totalHeight, transform.position.z);
            
        }

        private void UpdateGroundDetection()
        {
            var bottomStack = GetBottomStack();
            var bottomGroundDetection = bottomStack!.GetComponent<DurumaStack>().groundDetection;
            Debug.Log($"bottomStack: {bottomStack!.name}, bottomGroundDetection: {bottomGroundDetection}");
            if (!bottomStack)
            {
                groundDetection = bottomGroundDetection;
            }
            else
            {
                groundDetection.position = _startGroundDetectionPosition;
            }
        }
        
        [CanBeNull]
        private Transform GetBottomStack() => stacks <= 0 ? null : stackContainer.GetChild(stacks - 1);

        private void ShedStack()
        {
            if (stacks <= 0) return;
            
            // Destroy the last child.
            var lastChild = GetBottomStack();
            var stack = lastChild.GetComponent<DurumaStack>();
            stack.Die();
            UpdateGroundDetection();
        }
        
        public override void GotHit(IWeapon weapon)
        {
            if (weapon is Pie && stacks <= 0)
                Die();
        }
    }
}
