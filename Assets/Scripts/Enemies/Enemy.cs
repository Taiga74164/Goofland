using System;
using System.Collections.Generic;
using Controller;
using JetBrains.Annotations;
using Levels;
using Managers;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Enemies
{
    /// <summary>
    /// A struct that represents a currency drop.
    /// </summary>
    [Serializable]
    public struct CurrencyDrop
    {
        public CoinValue coinValue;
        public int quantity;
        
        public CurrencyDrop(CoinValue coinValue, int quantity)
        {
            this.coinValue = coinValue;
            this.quantity = quantity;
        }
    }
    
    /// <summary>
    /// Defines the base class for all enemies in the game.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [CanBeNull] public GameObject model;
    
        [Header("Enemy Settings")]
        [SerializeField] protected float speed;
        [SerializeField] protected int damage = 1;
        [SerializeField] private bool useTimer;
        [SerializeField] private float lineOfSight = 10.0f;
        
        [Header("Currency Drop Settings")]
        [SerializeField] private List<CurrencyDrop> currencyDrops;
        [SerializeField] private float dropForce = 5.0f;
        [SerializeField] private float dropRadius = 1.0f;
        
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
                player.TakeDamage(damage, transform);
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
            var playerTransform = GameManager.Instance.playerController.transform;
            var position = transform.position;
            var playerDirection  = playerTransform.position - position;
            var hit = Physics2D.Raycast(position, 
                playerDirection , lineOfSight, ~LayerMask.NameToLayer("Player"));
            
            // Draw the raycast in the Scene view.
            // Debug.DrawRay(transform.position, playerDirection * lineOfSight, Color.red);
            
            return hit.collider != null && hit.collider.IsPlayer();
        }

        protected internal virtual void Die()
        {
            DropCurrency();
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

        /// <summary>
        /// Drops currency when the enemy dies.
        /// </summary>
        private void DropCurrency()
        {
            foreach (var currencyDrop in currencyDrops)
            {
                for (var i = 0; i < currencyDrop.quantity; i++)
                {
                    // Create the currency prefab.
                    var obj = PrefabManager.Create(currencyDrop.coinValue.ToCurrencyPrefab());
                    
                    // Scatter within a radius.
                    var scatterPosition = Random.insideUnitSphere * dropRadius;
                    scatterPosition.y = 0;
                    // Set the position to the enemy's position plus a random scatter position.
                    obj.transform.position = transform.position + scatterPosition;
                    
                    // Add a random force to the currency.
                    var rbCoin = obj.GetComponent<Rigidbody2D>();
                    var forceDirection = (Random.insideUnitSphere + Vector3.up).normalized;
                    rbCoin.AddForce(forceDirection * dropForce, ForceMode2D.Impulse);
                    
                }
            }
        }
    }
}