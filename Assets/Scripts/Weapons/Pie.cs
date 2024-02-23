using System;
using System.Collections.Generic;
using Enemies;
using Levels;
using Managers;
using Objects.Scriptable;
using UnityEngine;
using Utils;

namespace Weapons
{
    [Serializable]
    public struct PieSkin
    {
        public string name;
        public Sprite sprite;
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pie : MonoBehaviour, IWeapon
    {
        [Header("Pie Settings")]
        public List<PieSkin> pieSkins = new List<PieSkin>();
        public Vector2 direction = new Vector2(1.0f, 3.0f);
        public Vector2 throwForce = new Vector2(5.0f, 5.0f);

        [Header("Audio Settings")]
        public GameEvent onImpact;
        [SerializeField] private AudioSource audioSource;

        [Header("Debugging")]
        public bool debug;
        private Vector3 _initialPosition;
        private float _travelDistance;
        
        private const float SpawnOffSet = 0.1f;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            // Set the initial position of the pie.
            transform.Translate(Vector3.up * SpawnOffSet);
            
            // Get the rigidbody and sprite renderer components.
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;
            
            // Get the sprite renderer.
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // Set Pie skin based on the current level.
            var levelName = LevelUtil.CurrentLevelName;
            foreach (var skin in pieSkins)
            {
                if (levelName.Contains(skin.name))
                {
                    _spriteRenderer.sprite = skin.sprite;
                    break;
                }
                else
                {
                    // Set the default pie skin.
                    _spriteRenderer.sprite = pieSkins[0].sprite;
                }
            }
        }

        private void Update()
        {
            // Rotate the pie to face the direction it's moving.
            var moveDirection = _rigidbody2D.velocity;
            var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            // Flip the pie sprite based on the direction it's moving.
            var shouldFlip = moveDirection.x < 0;
            _spriteRenderer.flipY = shouldFlip;
            transform.Rotate(0, 0, shouldFlip ? 90 : -90);
        }
        
        private void LateUpdate()
        {
            if (GameManager.IsPaused) return;
        
            // Destroy the pie if it falls off the map.
            if (transform.position.y < -100.0f) Die();

            #region Debugging

            if (debug) _travelDistance = Vector3.Distance(_initialPosition, transform.position);

            #endregion
        }
    
        private void OnCollisionEnter2D(Collision2D other)
        {
            // Play the impact sound.
            onImpact.Raise(audioSource);
            
            if (other.gameObject.CompareTag("Enemy"))
                other.gameObject.GetComponent<Enemy>().GotHit(this);
            else if (other.gameObject.CompareTag("Coin"))
                other.gameObject.GetComponent<Coin>().CollectCoin();
        
            // Stop the pie from moving on impact.
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            _spriteRenderer.enabled = false;
            Die();
            // Invoke(nameof(OnHit), audioSource.clip.length);
        }
    
        /// <summary>
        /// Throws the pie in the specified direction.
        /// </summary>
        /// <param name="playerVelocity">The player's velocity to add to the pie's force.</param>
        public void ThrowPie()
        {
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.AddForce(throwForce, ForceMode2D.Impulse);

            #region Debugging

            if (debug) _initialPosition = transform.position;

            #endregion
        }

        private void Die() => Destroy(gameObject);
    
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_initialPosition, transform.position);
        }
#endif
    }
}
