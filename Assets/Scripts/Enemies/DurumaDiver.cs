using System.Collections.Generic;
using System.Linq;
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
        
        [Header("DurumaDiver Audio Settings")]
        [SerializeField] private AudioData attackAudioData;

        private float _attackCooldown;
        private Transform _mainGroundDetection;
        private List<Transform> _stacks = new List<Transform>();

        protected override void Start()
        {
            base.Start();
            
            // Set the start ground detection position.
            _mainGroundDetection = groundDetection;

            // Generate the stacks.
            GenerateStack(stacks);
            
            // Set the next attack time.
            _attackCooldown = attackDelay;
        }

        protected override void Update()
        {
            base.Update();
            
            // Update the number of stacks.
            stacks = stackContainer.childCount;
            
            // Handle the DurumaDiver's attack.
            // HandleAttack();
        }
        
        protected override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer() && entityType == EntityType.Enemy)
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy: this);
                audioSource.PlayOneShot(attackAudioData.clip);
            }
            else if (other.gameObject.GetComponent<Pie>())
            {
                ShedStack();
            }
        }

        private void LateUpdate()
        {
            UpdateGroundDetection();
        }

        private void HandleAttack()
        {
            if (entityType is not EntityType.Enemy) return;
            
            // Decrement the next attack time.
            _attackCooldown -= Time.deltaTime;
            
            if (_attackCooldown <= 0 && PlayerInLineOfSight() && InAttackRange())
            {
                Attack();
                
                // Set the next attack time.
                _attackCooldown = attackDelay;
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
            Debug.Log($"Attacking!");
            rb.velocity = directionToPlayer * diveSpeed;
        }
        
        private void GenerateStack(int count)
        {
            float totalHeight = 0;
            for (var i = 0; i < count; i++)
            {
                // Create a new stack.
                var stack = PrefabManager.Create(Prefabs.DurumaDiverStack, stackContainer.transform);
                
                // Add stack to the list of stacks.
                _stacks.Add(stack.transform);
                
                // Calculate the stack's height.
                var stackHeight = stack.GetComponent<BoxCollider2D>().size.y * stack.transform.localScale.y;
                
                // Calculate the total height of the DurumaDiver.
                totalHeight -= i == 0 ? 
                    stackHeight / 2.0f : // Position the first stack at the center of the DurumaDiver.
                    stackHeight; // Position the subsequent stacks below the previous stack.

                // Set the position of the stack.
                stack.transform.localPosition = new Vector3(0, totalHeight, 0);
                stack.name = $"Stack {i + 1}";
            }

            // Set the position of the DurumaHead to the top of the stacks.
            transform.position = new Vector3(transform.position.x, -totalHeight, transform.position.z);
            
            // Update the ground detection of the DurumaDiver to the bottom stack's ground detection.
            UpdateGroundDetection();
        }

        private void UpdateGroundDetection()
        {
            if (_stacks.Count <= 0)
            {
                groundDetection = _mainGroundDetection;
                return;
            }
            
            // Sort the stacks by their y position so that the bottom stack is at the end of the list.
            // _stacks.Sort((a, b) => b.position.y.CompareTo(a.position.y));
            
            // Select the stack with the lowest y position.
            var bottomStack = _stacks.OrderBy(stack => stack.position.y).FirstOrDefault();
            if (!bottomStack) return;
            // Debug.Log($"{gameObject.name}: Bottom stack is {bottomStack.name}");
            
            var bottomGroundDetection = bottomStack!.GetComponent<DurumaStack>().groundDetection;
            groundDetection = bottomGroundDetection;
        }
        
        [CanBeNull]
        private Transform GetBottomStack() => stacks <= 0 ? null : stackContainer.GetChild(stacks - 1);

        private void ShedStack()
        {
            if (stacks <= 0) return;
            
            // Destroy the last child.
            var lastChild = GetBottomStack();
            var stack = lastChild!.GetComponent<DurumaStack>();
            
            // Remove the stack from the list of stacks.
            _stacks.Remove(lastChild);
            
            stack.OnHit();
        }
        
        public override void GotHit(IWeapon weapon)
        {
            if (weapon is Pie && stacks <= 0)
                OnHit();
        }
    }
}
