using System.Collections.Generic;
using System.Linq;
using Controllers;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Damage Settings")]
        public float damagePercentage = 5.0f;
        [SerializeField] protected int damage = 1;
    }
    
    /// <summary>
    /// Defines the base class for all enemies in the game.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : EnemyBase
    {
        [Header("Enemy Settings")]
        [SerializeField] protected EntityType entityType = EntityType.Enemy;
        [Tooltip("The current model of the enemy.")]
        [CanBeNull] public GameObject model;
        [Tooltip("The ally prefab to change into after getting hit.")]
        [CanBeNull] public GameObject allyPrefab;
        [SerializeField] protected float speed;
        
        [Header("Detection Settings")]
        [Tooltip("The length at which the enemy will detect the layer.")]
        [SerializeField]
        protected float rayLength = 1.0f;
        [SerializeField] protected internal Transform groundDetection;
        [Tooltip("The ground layer.")]
        [SerializeField]
        protected LayerMask groundLayer;
        [Tooltip("The layer at which the enemy will turn.")]
        [SerializeField]
        protected LayerMask turnLayer;
        [SerializeField] private float lineOfSight = 10.0f;
        [SerializeField] private LayerMask playerLayer;

        [Header("Currency Drop Settings")]
        [SerializeField] private List<CurrencyDrop> currencyDrops;
        [SerializeField] private float dropForce = 5.0f;
        [SerializeField] private float dropOffset = 1.0f;
        
        [Header("Weaknesses")]
        [SerializeField] private bool pieWeakness;
        [SerializeField] private bool pianoWeakness;
        
        protected Vector2 direction = Vector2.right;
        protected Rigidbody2D rb;
        protected Transform playerTransform;
    
        protected virtual void Awake()
        {
            // Register the enemy with the entity manager.
            EntityManager.Instance.RegisterEnemy(this);
            
            // Set the model to the game object if it is not set.
            model = model ? model : gameObject;
            
            // Get the rigidbody component.
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            // Get the player's transform.
            playerTransform = GameManager.Instance.playerController.transform;
        }

        protected virtual void Update() { }

        protected virtual void FixedUpdate()
        {
            if (GameManager.IsPaused) return;

            Patrol();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.IsPlayer() || entityType is not EntityType.Enemy) return;
            
            // Deal damage to the player.
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(enemy: this);
        }

        /// <summary>
        /// Moves the enemy in the direction it is facing.
        /// </summary>
        protected virtual void Patrol()
        {
            // Move the enemy in the direction it is facing.
            transform.Translate(direction * (speed * Time.deltaTime));

            var groundPosition = groundDetection.position;
            var groundForward = groundPosition + new Vector3(direction.x * rayLength, 0, 0);
            // We use -0.5f on the y since our tile is 0.5f in height.
            var down = new Vector2(0, -0.5f);
            
            // We cast a ray from the ground detection position to the direction the enemy is facing.
            var groundInfo = Physics2D.Raycast(groundForward, 
                down.Add(groundForward), rayLength, groundLayer);
            // We cast a ray from the ground detection position to the direction the enemy is facing.
            var wallInfo = Physics2D.Raycast(groundPosition, direction, rayLength, turnLayer);
            
            // Draw ground info.
            Debug.DrawLine(groundForward, groundForward.Add(down), Color.yellow);
            // Draw wall info.
            Debug.DrawLine(groundPosition, groundPosition.Add(direction * rayLength), Color.red);

            if (!groundInfo.collider || wallInfo.collider)
                Turn();
        }
        
        /// <summary>
        /// Turns the enemy around.
        /// </summary>
        protected virtual void Turn()
        {
            direction *= new Vector2(-1, 0);
            if (direction == Vector2.left)
            {
                // Rotate the enemy 180 degrees.
                model!.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (direction == Vector2.right)
            {
                // Reset the enemy's rotation.
                model!.transform.eulerAngles = Vector3.zero;
            }
        }
        
        /// <summary>
        /// Checks if the player is in the enemy's line of sight.
        /// </summary>
        /// <returns>True if the player is in the enemy's line of sight.</returns>
        protected bool PlayerInLineOfSight()
        {
            if (entityType is not EntityType.Enemy) return false;
            
            // Get the player's position and direction.
            var playerTransform = GameManager.Instance.playerController.transform;
            var position = transform.position;
            var playerDirection  = playerTransform.position - position;
            // Cast a ray to check if the player is in the enemy's line of sight.
            var hit = Physics2D.Raycast(position, playerDirection, lineOfSight, playerLayer);
            
            // Draw the raycast in the Scene view.
            // Debug.DrawLine(transform.position, playerDirection * lineOfSight, Color.red);
            
            return hit.collider != null && hit.collider.IsPlayer();
        }

        protected internal virtual void OnHit()
        {
            if (entityType is not EntityType.Enemy) return;
            
            // Drop dice when the enemy is hit.
            DropDice();
            
            // Play particle effects.
            
            // Change the enemy's state.
            entityType = EntityType.Ally;
            
            // Unregister the enemy with the entity manager.
            EntityManager.Instance.UnregisterEnemy(this);
            
            // Change the model to the ally prefab.
            if (allyPrefab != null)
            {
                // Instantiate the ally prefab.
                var ally = Instantiate(allyPrefab, transform);
                // Destroy the current model.
                if (model) Destroy(model);
                // Store the current model's euler angles.
                var eulerAngles = model!.transform.eulerAngles;
                // Assign the new model to the ally prefab.
                model = ally;
                // Set the new model's euler angles.
                model!.transform.eulerAngles = eulerAngles;
                // Set the new model's position.
                gameObject.layer = LayerMask.NameToLayer("Ally");
                // Include the player layer in the excluded layers of the rigidbody.
                rb.excludeLayers |= playerLayer;
                // Include Projectile layer in the excluded layers of the collider.
                gameObject.GetComponents<Collider2D>().ToList().ForEach(col => 
                    col.excludeLayers |= 1 << LayerMask.NameToLayer("Projectile"));
            }
        }

        /// <summary>
        /// Called when the enemy is hit by a weapon.
        /// </summary>
        /// <param name="weapon">The weapon type.</param>
        public virtual void GotHit(IWeapon weapon)
        {
            if (entityType is not EntityType.Enemy) return;
            
            if ((weapon is Pie && pieWeakness) || (weapon is Piano && pianoWeakness))
                OnHit();
        }
        
        protected void DropDice() => currencyDrops.ForEach(currencyDrop => 
            CurrencyManager.DropCurrency(currencyDrop.coinValue, currencyDrop.quantity, 
                dropForce, dropOffset, transform.position, direction));
    }

    public enum EntityType
    {
        Enemy,
        Ally
    }
}