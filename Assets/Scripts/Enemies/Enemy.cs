using System;
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
    public class Enemy : MonoBehaviour
    {
        [CanBeNull] public GameObject model;
    
        [Header("Enemy Settings")]
        [SerializeField] private float speed;
        [SerializeField] private int damage = 1;


        [Header("Weaknesses")]
        [SerializeField] private bool pieWeakness;
        [SerializeField] private bool bananaWeakness;
        [SerializeField] private bool waterWeakness;
    
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
                var player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage();
                player.KnockBack(transform);
            }
            else if (collision.gameObject.layer != ~LayerMask.NameToLayer("Player"))
            {
                Turn();
            }
        }

        protected virtual void MoveEnemy() => transform.Translate(direction * (speed * Time.deltaTime));

        /// <summary>
        /// Moves the enemy in the opposite direction after a certain amount of time.
        /// </summary>
        private void Timer()
        {
            _turnCount += Time.deltaTime;
            if (!(_turnCount >= _turnTimer)) return;
            Turn();
            _turnCount = 0;
        }
        
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

        protected virtual void Die() => Destroy(gameObject);

        /// <summary>
        /// Called when the enemy is hit by a weapon.
        /// </summary>
        /// <param name="weapon">The weapon type.</param>
        public virtual void GotHit(IWeapon weapon)
        {
            var isWeak = (weapon is Pie && pieWeakness) || 
                         (weapon is BananaPeel && bananaWeakness) ||
                         (weapon is WaterGunProjectile && waterWeakness);
        
            if (isWeak) Die();
        }
    }
}