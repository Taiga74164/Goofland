using Controller;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using Weapons;

namespace Enemies
{
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
        [SerializeField] private float lineOfSight = 10.0f;

        [Header("Weaknesses")]
        [SerializeField] private bool pieWeakness;
        [SerializeField] private bool pianoWeakness;
    
        protected Vector2 direction = Vector2.left;
        private bool _useTimer;
        private float _turnTimer;
        private float _turnCount;
    
        protected virtual void Awake()
        {
            model = model ? model : gameObject;
        }
        
        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
        
            if (_useTimer) Timer();

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
            var playerDirection  = playerTransform.position - transform.position;
            var hit = Physics2D.Raycast(transform.position, 
                playerDirection , lineOfSight, 
                ~LayerMask.NameToLayer("Player"));
            return hit.collider != null && hit.collider.IsPlayer();
        }

        protected virtual void Die() => Destroy(gameObject);

        /// <summary>
        /// Called when the enemy is hit by a weapon.
        /// </summary>
        /// <param name="weapon">The weapon type.</param>
        public virtual void GotHit(IWeapon weapon)
        {
            if ((weapon is Pie && pieWeakness) || (weapon is Piano && pianoWeakness))
                Die();
        }
    }
}