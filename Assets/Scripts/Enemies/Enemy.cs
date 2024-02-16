using System.Collections.Generic;
using Controllers;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public float damagePercentage = 5.0f;
    }
    
    /// <summary>
    /// Defines the base class for all enemies in the game.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : EnemyBase
    {
        [CanBeNull] public GameObject model;
    
        [Header("Enemy Settings")]
        [SerializeField] protected float speed;
        [SerializeField] protected int damage = 1;
        [SerializeField] private bool useTimer;
        [SerializeField] private float lineOfSight = 10.0f;
        [SerializeField] private LayerMask playerLayer;
        
        [Header("Currency Drop Settings")]
        
        [SerializeField] private List<CurrencyDrop> currencyDrops;
        [SerializeField] private float dropForce = 5.0f;
        [SerializeField] private float scatterRadius = 1.0f;
        
        [Header("Weaknesses")]
        [SerializeField] private bool pieWeakness;
        [SerializeField] private bool pianoWeakness;
    
        protected Vector2 direction = Vector2.left;
        protected Rigidbody2D rb;
        private float _turnTimer;
        private float _turnCount;
    
        protected virtual void Awake()
        {
            model = model ? model : gameObject;
            rb = GetComponent<Rigidbody2D>();
        }
        
        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
        
            if (useTimer) Timer();

            MoveEnemy();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.IsPlayer())
            {
                // Deal damage to the player.
                var player = collision.gameObject.GetComponent<PlayerController>();
                // player.TakeDamage(damage, transform);
                player.DropCurrency(this);
            }
            else if (collision.gameObject.layer != ~LayerMask.NameToLayer("Player"))
            {
                // Turn around if the enemy hits a wall or ledge.
                Turn();
            }
        }

        /// <summary>
        /// Moves the enemy in the direction it is facing.
        /// </summary>
        protected virtual void MoveEnemy() => transform.Translate(direction * (speed * Time.deltaTime));

        /// <summary>
        /// Moves the enemy in the opposite direction after a certain amount of time.
        /// </summary>
        protected virtual void Timer()
        {
            _turnCount += Time.deltaTime;
            if (!(_turnCount >= _turnTimer)) return;
            Turn();
            _turnCount = 0;
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
            // Get the player's position and direction.
            var playerTransform = GameManager.Instance.playerController.transform;
            var position = transform.position;
            var playerDirection  = playerTransform.position - position;
            // Cast a ray to check if the player is in the enemy's line of sight.
            var hit = Physics2D.Raycast(position, playerDirection, lineOfSight, playerLayer);
            
            // Draw the raycast in the Scene view.
            // Debug.DrawRay(transform.position, playerDirection * lineOfSight, Color.red);
            
            return hit.collider != null && hit.collider.IsPlayer();
        }

        protected internal virtual void Die()
        {
            DropDice();
            Destroy(gameObject);
        }

        /// <summary>
        /// Called when the enemy is hit by a weapon.
        /// </summary>
        /// <param name="weapon">The weapon type.</param>
        public virtual void GotHit(IWeapon weapon)
        {
            if ((weapon is Pie && pieWeakness) || (weapon is Piano && pianoWeakness))
                Die();
        }
        
        private void DropDice()
        {
            currencyDrops.ForEach(currencyDrop => CurrencyManager.DropCurrency(
                currencyDrop.coinValue, currencyDrop.quantity, dropForce, scatterRadius, transform.position));
        }
    }
}